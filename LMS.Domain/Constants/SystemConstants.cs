namespace LMS.Domain.Constants;

public static class SystemConstants
{
    // Vocabulary Config
    public const string SystemPrefix = "sys";
    public const string SystemNamespace = "http://schema.lms.com/system#";
    public const string SystemLabel = "System Internal Vocabulary";

    // Structural Properties 
    public const string IsMemberOf = "isMemberOf";    
    public const string IsMemberOfUri = "http://schema.lms.com/system#isMemberOf";
    public const string HasMedia = "hasMedia";        
    public const string HasThumbnail = "hasThumbnail"; 
    
    // Value Types
    public const string TypeResource = "resource"; 
    public const string TypeText = "text";         
    public const string TypeUri = "uri";           
}