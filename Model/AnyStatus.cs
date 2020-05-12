using System;

namespace StackErp.Model
{
public struct AnyStatus {
    public string Message;
    public int Code;
        public static implicit operator int(AnyStatus other)
        {
            return other.Code;
        }

        public static implicit operator AnyStatus(int other)
        {
            AnyStatus error;

            error.Code = other;
            error.Message = null;

            return error;
        }

        public override bool Equals(object obj)
        {
            return (int)obj == Code;
        }

        public override int GetHashCode()
        {
            if (Message == null)
            {
                return Code;
            }

            return Message.GetHashCode() ^ Code;
        }

    public const int Success = 0;
    public const int NotInitialized = 1;
    public const int SaveFailure = 2;
    public const int SelectFailure = 3;
    public const int UpdateFailure = 4;
    public const int InvalidData = 9;
    public const int InSuficientData = 10;
    public const int PermissionDenied = 11;
    public const int InvalidPostData = 20;
    public const int InvalidUrl = 30;
     public const int InvalidScript = 31;
     public const int ScriptFailure = 32;
}
}