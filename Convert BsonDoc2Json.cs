using System.Collections.Generic;
using System.Text;

public static LogDocViewModel ToLogDoc(this LogCollectionViewModel log)
{
    return new LogDocViewModel()
    {
        Description = log.Description,        
        Message = log?.LogDoc?["Exception"]?["Message"].AsString,
        StackTrace = log?.LogDoc?["Exception"]?["StackTraceString"].AsString,
        Source = log?.LogDoc?["Exception"]?["Source"].AsString,
        ParamData = (log?.LogDoc?["Data"].AsBsonValue).ToString().Replace("\"", "")
    };
}



public static string ConvertBsonDocumentToFormattedString(BsonDocument? logDoc)
{
    if (logDoc == null)
    {
        return null;
    }
    var stringBuilder = new StringBuilder();

    foreach (var element in logDoc.Elements)
    {
        string field = element.Name;
        string value = element.Value.ToString();
        stringBuilder.AppendLine($"{field}: {value}");
    }

    return stringBuilder.ToString().Replace("\r\n", "  ");
}

public static string ConvertBsonDocumentToFormattedString2(BsonDocument logDoc)
{
    var stringBuilder = new StringBuilder();
    AppendBsonElements(logDoc, stringBuilder, indentLevel: 0);
    return stringBuilder.ToString();
}

private static void AppendBsonElements(BsonDocument doc, StringBuilder stringBuilder, int indentLevel)
{
    string indent = new string(' ', indentLevel * 2); // Create indentation based on the nesting level

    foreach (var element in doc.Elements)
    {
        string field = element.Name;

        if (element.Value is BsonDocument nestedDoc)
        {
            // If the value is a nested document, recursively append its fields
            stringBuilder.AppendLine($"{indent}{field}:");
            AppendBsonElements(nestedDoc, stringBuilder, indentLevel + 1);
        }
        else if (element.Value is BsonArray array)
        {
            // If the value is an array, iterate over its elements
            stringBuilder.AppendLine($"{indent}{field}: [");
            foreach (var arrayElement in array)
            {
                if (arrayElement is BsonDocument arrayNestedDoc)
                {
                    // Recursively handle nested documents in the array
                    AppendBsonElements(arrayNestedDoc, stringBuilder, indentLevel + 1);
                }
                else
                {
                    // Handle simple values in the array
                    stringBuilder.AppendLine($"{indent}  - {arrayElement}");
                }
            }
            stringBuilder.AppendLine($"{indent}]");
        }
        else
        {
            // For simple values, convert to string directly
            string value = element.Value.ToString();
            stringBuilder.AppendLine($"{indent}{field}: {value}");
        }
    }
}


public static string? ConvertDocToObj(BsonDocument? doc)
{
    if (doc == null)
    {
        return null;
    }

    var dynamicObject = new Dictionary<string, object>();

    foreach (var element in doc.Elements)
    {
        dynamicObject[element.Name] = ConvertBsonValueToDynamic(element.Value);
        //dynamicObject[element.Name] = element.Value.ToString();
    }

    return JsonConvert.SerializeObject(dynamicObject);
}

private static object ConvertBsonValueToDynamic(BsonValue bsonValue)
{
    switch (bsonValue.BsonType)
    {
        case BsonType.Document:
            return bsonValue.AsBsonDocument.ToDictionary(); // Convert nested document
        case BsonType.Array:
            return bsonValue.AsBsonArray.Select(ConvertBsonValueToDynamic).ToList(); // Convert array
        default:
            return bsonValue.ToString(); // Convert primitive types
    }
}
