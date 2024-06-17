namespace AndroidWebService.Models.Enums
{
    using AndroidWebService.Models.Utils;

    public class MediaPath
    {
        public static readonly string 
                MOTEL_THUMBNAIL_PATH = ConfigParser.Parse("motel_thumbnail_path");
        public static readonly string
                MOTEL_MEDIA_PATH = ConfigParser.Parse("motel_media_path");
        public static readonly string
                USER_AVATAR_PATH = ConfigParser.Parse("user_avatar_path");
        public static readonly string
                USER_DEFAULT_AVATAR_PATH = ConfigParser.Parse("user_default_avatar_path");
        public static readonly string
                USER_DEFAULT_AVATAR_FILE_NAME = "defaultProfilePicture.png";
    }
}
