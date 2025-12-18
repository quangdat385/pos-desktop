namespace PosApi.Shared
{
    // Enum đại diện cho mã lỗi ứng dụng
    public enum AppErrorCode
    {
        None = 0,
        BAD_REQUEST = 1001,
        NOT_FOUND = 1002,
        UNAUTHORIZED_ACCESS = 1003,
        FORBIDDEN = 1004,
        INTERNAL_SERVER_ERROR = 1005,
        UNAUTHENTICATED = 1006,
        TOKEN_EXPIRED = 1007,
        VALIDATION_ERROR = 1008,
    }
}
