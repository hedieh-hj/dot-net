using Dapper;
using ota.barn.model.Models;
using ota.context.Contexts;
using ota.context.ParameterContext;
using ota.core.Base;
using ota.core.Models;
using ota.setting.model.ViewModels;
using System.Reflection;

namespace ota.setting.repository.Repositories
{
    // you can use this class to get a generic method with a model_name 
    public class ReflectionSetting
    {               
        public async Task<object?> GetAsync(string model_name)
        {
            try
            {                
                // Get the type of the GlobalSettingViewModel
                Type globalSettingType = typeof(GlobalSettingViewModel);

                // Find the property based on the input string `t`
                PropertyInfo? propertyInfo = globalSettingType.GetProperty(model_name);

                if (propertyInfo == null)
                {
                    throw new Exception($"Property '{model_name}' does not exist in GlobalSettingViewModel.");
                }

                // Get the type of the list (e.g., List<model_name>)
                Type? listType = propertyInfo.PropertyType.GetGenericArguments()[0];                               

                // Prepare and execute the query with the dynamically determined type
                var query = "SELECT * FROM GetX()";

                var queryMethod = typeof(SqlMapper)
                   .GetMethods()
                   .First(m => m.Name == "QueryAsync" && m.IsGenericMethod);

                // Make the generic version of QueryAsync for the correct type
                var genericQueryMethod = queryMethod.MakeGenericMethod(listType.GenericTypeArguments[0]);

                // Prepare the parameters
                var parameters = new object[] {
                    _context.Connection,
                    query,
                    null, // parameters
                    null, // transaction
                    null, // commandTimeout
                    null  // commandType
                };

                // Invoke the method dynamically to get the result
                var resultTask = (Task)genericQueryMethod.Invoke(null, parameters);

                // Await the result task
                await resultTask.ConfigureAwait(false);

                // Extract the result from the completed Task object
                var resultProperty = resultTask.GetType().GetProperty("Result");
                var result = resultProperty?.GetValue(resultTask);

                // Check if result is a list and return it
                if (result is IEnumerable<PaymentGatewaysModel> resultList)
                {
                    return resultList.ToList();
                }
                else
                {
                    Console.WriteLine("Query returned no items or the wrong type.");
                    return null;
                }

            }
            catch (Exception e)
            {
                throw new ExceptionModel("Error retrieving information", e);
            }
        }
    }
}
