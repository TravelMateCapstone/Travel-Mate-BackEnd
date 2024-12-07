namespace BusinessObjects.Utils.Response
{
    public record Response(
     int error,
     String message,
     object? data
 );
}
