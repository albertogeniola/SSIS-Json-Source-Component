using com.webkingsoft.JsonSuite.Component.Exceptions;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.webkingsoft.JsonSuite.Component.Model
{
    public class AttributeMappingOptions
    {
        private JsonAttributeType _type;
        private int _scale;
        private int _precision;

        
        public AttributeMappingOptions(JsonAttributeType type, int scale, int precision) {
            _type = type;
            _scale = scale;
            _precision = precision;
        }
        
        public int SSISScale {
            // If the type is number, allow the user to se it. In any other case, assume it's 0.
            get {
                switch (_type) {
                    case JsonAttributeType.Number:
                        return _scale;
                    default:
                        return 0;
                }
            }
            set {
                if (value < 0 || value > 28)
                    throw new InvalidAtributeValueException("The type " + _type + " cannot have a scale of " + value + ". Scale must be an integer value between 0 and 28 included.");

                switch (_type)
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

        public int SSISPrecision {
            // If the type is number, allow the user to se it. In any other case, assume it's 0.
            get
            {
                switch (_type)
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
                    throw new InvalidAtributeValueException("The type " + _type + " cannot have a precision of " + value + ". Precision must be an integer value between 0 and 28 included.");
                switch (_type)
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

        public int SSISLength {
            get {
                return 0;
            }
        }

        public int SSISCodePage {
            get {
                return 0;
            }
        }

        public DataType GetSSISDataType()
        {
            switch (_type)
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
