public class AnyStatus {
    public string Message {set;get;}
    public int Code {set;get;}

    public AnyStatus(int code, string message = null) {
        this.Code = code;
        this.Message = message;
    }

    public static AnyStatus Success => new AnyStatus(0);
    public static AnyStatus NotInitialized => new AnyStatus(1);
    public static AnyStatus SaveFailure => new AnyStatus(2);
    public static AnyStatus SelectFailure => new AnyStatus(3);
    public static AnyStatus UpdateFailure => new AnyStatus(4);
    public static AnyStatus InvalidData => new AnyStatus(9);
    public static AnyStatus InSuficientData => new AnyStatus(10);
    public static AnyStatus PermissionDenied => new AnyStatus(11);
    public static AnyStatus InvalidPostData => new AnyStatus(20);
    public static AnyStatus InvalidUrl => new AnyStatus(30);
}