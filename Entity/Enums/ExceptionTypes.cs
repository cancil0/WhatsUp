using System.ComponentModel;

namespace Entity.Enums
{
    public enum ExceptionTypes : int
    {
        [Value("400"), Description("BadRequest")]
        BadRequest = 400,

        [Value("401"), Description("UnAuthorized")]
        UnAuthorized = 401,

        [Value("404"), Description("NotFound")]
        NotFound = 404,

        [Value("405"), Description("NotAllowed")]
        NotAllowed = 405,

        [Value("500"), Description("NotAllowed")]
        InternalServerError = 500

    }
}
