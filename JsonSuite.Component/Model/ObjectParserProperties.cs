using com.webkingsoft.JsonSuite.Component.Exceptions;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.webkingsoft.JsonSuite.Component.Model
{
    public class ObjectParserProperties : AbstractCustomPropertyHelper
    {
        private static readonly string INPUT_COLUMN_NAME = "InputColumnName";
        private static readonly string INPUT_COLUMN_NAME_DESC = "Name of the column, belonging to the input lane, where the raw json is coming from.";

        private static readonly string OUTPUT_COLUMNS = "OutputColumnsNames";
        private static readonly string OUTPUT_COLUMNS_DESC = "Names of the output columns that are associated to parsed Json Object Attributes";

        private static readonly string OBJECT_ATTRIBUTES_EXPR = "JsonPathExpressions";
        private static readonly string OBJECT_ATTRIBUTES_EXPR_DESC = "JPath expressions to extract Json Object Attributes";

        private static readonly string ATTRIBUTES_TYPES = "AttributesTypes";
        private static readonly string ATTRIBUTES_TYPES_DESC = "Types of the Json Object attributes.";

        private static readonly string ATTRIBUTES_SCALE = "AttributesScale";
        private static readonly string ATTRIBUTES_SCALE_DESC = "Integer value for attribute scale";

        private static readonly string ATTRIBUTES_PRECISION = "AttributesPrecision";
        private static readonly string ATTRIBUTES_PRECISION_DESC = "Integer value for attribute precision";

        private static readonly string ATTRIBUTES_LENGTH = "AttributesLength";
        private static readonly string ATTRIBUTES_LENGTH_DESC = "Integer value for attribute length";

        private static readonly string ATTRIBUTES_CODEPAGE = "AttributesCodepage";
        private static readonly string ATTRIBUTES_CODEPAGE_DESC = "Integer value for attribute codepage";

        /// <summary>
        /// The name of the input column that holds the raw json to parse
        /// </summary>
        public string RawJsonInputColumnName { get; set; }

        /// <summary>
        /// Mapping between the output column names and the json attributes to parse.
        /// The key of the dictionary is the name of the output column. The associated
        /// value is the configuration of the JsonAttribute (expression + expected type)
        /// </summary>
        public Dictionary<string, AttributeMapping> AttributeMappings = new Dictionary<string, AttributeMapping>();

        protected override void InternalCopyToComponent(IDTSCustomPropertyCollection100 componentCustomProperties)
        {
            // Create the raw column-name property
            var p = CreateCustomProperty(componentCustomProperties, INPUT_COLUMN_NAME, INPUT_COLUMN_NAME_DESC);
            p.Value = RawJsonInputColumnName;

            // Now serialize every item of the mapping dictionary into a separate array
            List<string> attributesExpressions = new List<string>();
            List<JsonAttributeType> attributesTypes = new List<JsonAttributeType>();
            List<string> outputColumns = new List<string>();

            // Internal SSIS type info
            List<int> attributesScale = new List<int>();
            List<int> attributesPrecision = new List<int>();
            List<int> attributesLength = new List<int>();
            List<int> attributesCodepage = new List<int>();

            // Create a tuple of items for every AttributeMapping
            foreach (KeyValuePair<string, AttributeMapping> mapping in AttributeMappings) {
                outputColumns.Add(mapping.Key);
                attributesExpressions.Add(mapping.Value.ObjectAttributeSelectionExpression);
                attributesTypes.Add(mapping.Value.AttributeType);
                attributesScale.Add(mapping.Value.SSISScale);
                attributesPrecision.Add(mapping.Value.SSISPrecision);
                attributesLength.Add(mapping.Value.SSISLength);
                attributesCodepage.Add(mapping.Value.SSISCodePage);
            }

            // Serialize that data. Simple arrays of strings/integers are automatically marshalled/unmarshalled.
            var attrExpr = CreateCustomProperty(componentCustomProperties, OBJECT_ATTRIBUTES_EXPR, OBJECT_ATTRIBUTES_EXPR_DESC);
            attrExpr.Value = attributesExpressions.ToArray();

            var attrTypes = CreateCustomProperty(componentCustomProperties, ATTRIBUTES_TYPES, ATTRIBUTES_TYPES_DESC);
            attrTypes.Value = attributesTypes.ToArray();

            var ocols = CreateCustomProperty(componentCustomProperties, OUTPUT_COLUMNS, OUTPUT_COLUMNS_DESC);
            ocols.Value = outputColumns.ToArray();

            var scales = CreateCustomProperty(componentCustomProperties, ATTRIBUTES_SCALE, ATTRIBUTES_SCALE_DESC);
            scales.Value = attributesScale.ToArray();

            var precisions = CreateCustomProperty(componentCustomProperties, ATTRIBUTES_PRECISION, ATTRIBUTES_PRECISION_DESC);
            precisions.Value = attributesPrecision.ToArray();

            var lengths = CreateCustomProperty(componentCustomProperties, ATTRIBUTES_LENGTH, ATTRIBUTES_LENGTH_DESC);
            lengths.Value = attributesLength.ToArray();

            var codepages = CreateCustomProperty(componentCustomProperties, ATTRIBUTES_CODEPAGE, ATTRIBUTES_CODEPAGE_DESC);
            codepages.Value = attributesCodepage.ToArray();
        }

        public override void LoadFromComponent(IDTSCustomPropertyCollection100 componentCustomProperties)
        {
            // First of all, load the input column name where to fetch the json from
            RawJsonInputColumnName = GetCustomProperty(componentCustomProperties, INPUT_COLUMN_NAME) as string;

            // Now fetch the expressions, attributes and outputcolumns
            var outputCols = GetCustomProperty(componentCustomProperties, OUTPUT_COLUMNS) as string[];
            var exprs = GetCustomProperty(componentCustomProperties, OBJECT_ATTRIBUTES_EXPR) as string[];
            var types = GetCustomProperty(componentCustomProperties, ATTRIBUTES_TYPES) as JsonAttributeType[];
            var scales = GetCustomProperty(componentCustomProperties, ATTRIBUTES_SCALE) as int[];
            var precisions= GetCustomProperty(componentCustomProperties, ATTRIBUTES_PRECISION) as int[];
            var lengths = GetCustomProperty(componentCustomProperties, ATTRIBUTES_LENGTH) as int[];
            var codepages = GetCustomProperty(componentCustomProperties, ATTRIBUTES_CODEPAGE) as int[];

            // TODO: handle error cases when the user manually messes up with the custom properties
            Dictionary<string, AttributeMapping> res = new Dictionary<string, AttributeMapping>();
            for (int i = 0; i < outputCols.Length; i++) {
                AttributeMapping mapping = new AttributeMapping(exprs[i], types[i], scales[i], precisions[i]);
                res.Add(outputCols[i], mapping);
            }

            // Load the parsed data into the object public accessible property
            AttributeMappings = res;
        }

        public class AttributeMapping {
            private int _scale;
            private int _precision;

            /// <summary>
            /// The JPath expression to select the JSON attribute within the input object. 
            /// In the most simple case, that's just the attribute name.
            /// </summary>
            public string ObjectAttributeSelectionExpression { get; set; }
                    
            public AttributeMapping(string attributeSelectionExpression, JsonAttributeType type, int scale, int precision)
            {
                AttributeType = type;
                ObjectAttributeSelectionExpression = attributeSelectionExpression;
                _scale = scale;
                _precision = precision;
            }

            public JsonAttributeType AttributeType
            {
                get;
            }

            public int SSISScale
            {
                // If the type is number, allow the user to se it. In any other case, assume it's 0.
                get
                {
                    switch (AttributeType)
                    {
                        case JsonAttributeType.Number:
                            return _scale;
                        default:
                            return 0;
                    }
                }
                set
                {
                    if (value < 0 || value > 28)
                        throw new InvalidAtributeValueException("The type " + AttributeType + " cannot have a scale of " + value + ". Scale must be an integer value between 0 and 28 included.");

                    switch (AttributeType)
                    {
                        case JsonAttributeType.Number:
                            _scale = value;
                            break;
                        default:
                            _scale = 0;
                            break;
                    }
                }
            }

            public int SSISPrecision
            {
                // If the type is number, allow the user to se it. In any other case, assume it's 0.
                get
                {
                    switch (AttributeType)
                    {
                        case JsonAttributeType.Number:
                            return _precision;
                        default:
                            return 0;
                    }
                }
                set
                {
                    if (value < 0 || value > 28)
                        throw new InvalidAtributeValueException("The type " + AttributeType + " cannot have a precision of " + value + ". Precision must be an integer value between 0 and 28 included.");
                    switch (AttributeType)
                    {
                        case JsonAttributeType.Number:
                            _precision = value;
                            break;
                        default:
                            _precision = 0;
                            break;
                    }
                }
            }

            public int SSISLength
            {
                get
                {
                    return 0;
                }
            }

            public int SSISCodePage
            {
                get
                {
                    return 0;
                }
            }

            public DataType SSISDataType
            {
                get
                {
                    switch (AttributeType)
                    {
                        // For numbers, provide Numeric
                        case JsonAttributeType.Number:
                            return DataType.DT_NUMERIC;

                        // For booleans, return the corresponding type
                        case JsonAttributeType.Boolean:
                            return DataType.DT_BOOL;

                        // For array, objects, strings and any other, return DT_NEXT
                        case JsonAttributeType.Array:
                        case JsonAttributeType.Object:
                        case JsonAttributeType.String:
                        case JsonAttributeType.Other:
                        default:
                            return DataType.DT_NTEXT;
                    }
                }
            }
        }
    }
}
