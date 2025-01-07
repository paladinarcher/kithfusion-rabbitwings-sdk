namespace RabbitWings.Core
{
	public enum ErrorType
	{
		Undefined,

        RequestError,

        ServerError,

        UnknownError,
		NetworkError,

		InvalidToken,
		AuthorizationHeaderNotSent,

		MethodIsNotAllowed,
		NotSupportedOnCurrentPlatform,

		InvalidData,
		ProductDoesNotExist,
		PayStationServiceException,
		UserNotFound,
		CartNotFound,
		OrderNotFound,
		InvalidCoupon,

		PasswordResetNotAllowedForProject,
		RegistrationNotAllowedException,
		TokenVerificationException,
		UsernameIsTaken,
		EmailIsTaken,
		UserIsNotActivated,
		CaptchaRequiredException,
		InvalidProjectSettings,
		InvalidLoginOrPassword,
		InvalidAuthorizationCode,
		ExceededAuthorizationCodeAttempts,
		MultipleLoginUrlsException,
		SubmittedLoginUrlNotFoundException,

		IncorrectFriendState,

		TimeLimitReached
	}
}