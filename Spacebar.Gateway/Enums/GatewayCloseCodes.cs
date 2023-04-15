namespace Spacebar.Static.Enums;

public enum GatewayCloseCodes
{
    UnknownError = 4000,
    UnknownOpcode,
    DecodeError,
    NotAuthenticated,
    AuthenticationFailed,
    AlreadyAuthenticated,
    InvalidSession,
    InvalidSeq,
    RateLimited,
    SessionTimedOut,
    InvalidShard,
    ShardingRequired,
    InvalidApiVersion,
    InvalidIntent,
    DisallowedIntent
}