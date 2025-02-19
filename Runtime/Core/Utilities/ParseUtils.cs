using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;

namespace RabbitWings.Core
{
	public static class ParseUtils
	{
		private static readonly JsonSerializerSettings serializerSettings;

		static ParseUtils()
		{
			serializerSettings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore
			};
		}

		public static string ToJson<TData>(TData data) where TData : class
		{
			return JsonConvert.SerializeObject(data, serializerSettings);
		}

		public static TData FromJson<TData>(string json) where TData : class
		{
			TData result;

			try
			{
				result = JsonConvert.DeserializeObject<TData>(json, serializerSettings);
			}
			catch (Exception e)
			{
				XDebug.LogWarning($"Deserialization failed for {typeof(TData)}");
				result = null;
			}

			return result;
		}

		private static Error ParseError(long statusCode, string json)
		{
			if (statusCode > 299)
			{
				SimpleError se = FromJson<SimpleError>(json);
				Error e = new Error();
				e.statusCode = statusCode.ToString();
				e.errorMessage = se.message;
				e.ErrorType = ErrorType.UnknownError;
				e.errorCode = e.statusCode;
				XDebug.LogError(ToJson<Error>(e));
				return e;
			}
			if (json.Contains("statusCode") && json.Contains("errorCode") && json.Contains("errorMessage"))
				return FromJson<Error>(json);

			if (json.Contains("error") && json.Contains("code") && json.Contains("description"))
				return FromJson<LoginError>(json).ToError();

			return null;
		}

		public static bool TryParseError(long responseCode, string json, out Error error)
		{
			if (string.IsNullOrEmpty(json))
			{
				error = null;
				return false;
			}

			try
			{
				error = ParseError(responseCode, json);
				return error != null;
			}
			catch (Exception ex)
			{
				error = new Error(ErrorType.InvalidData, errorMessage: ex.Message);
				return true;
			}
		}

		public static bool TryGetValueFromUrl(string url, ParseParameter parameter, out string value)
		{
			var parameterName = parameter.ToString();
			var regex = new Regex($"[&?]{parameterName}=[a-zA-Z0-9._+-]+");
			value = regex.Match(url)
				.Value
				.Replace($"{parameterName}=", string.Empty)
				.Replace("&", string.Empty)
				.Replace("?", string.Empty);

			switch (parameter)
			{
				case ParseParameter.error_code:
				case ParseParameter.error_description:
					value = value?.Replace("+", " ");
					break;
				default:
					XDebug.Log($"Trying to find {parameterName} in URL:{url}");
					break;
			}

			return !string.IsNullOrEmpty(value);
		}
	}
}