using System.Text.RegularExpressions;

namespace SourceCodeGeneration.Data;

/// <summary>
/// 列名
/// </summary>
public class ColumnName
{
	#region Constructors

	/// <summary>
	/// 列名を初期化します。
	/// </summary>
	/// <param name="value">値</param>
	public ColumnName(string value)
	{
		bool success = Validate(value, out string message);
		if (!success) throw new ArgumentException(message, nameof(value));

		SnakeCase = value;

		{
			string[] lowerWords = value.Split('_');
			string[] upperWords = lowerWords.Select(x => x[0..1].ToUpper() + x[1..]).ToArray();
			string secondAfterUpperWords = string.Join(string.Empty, upperWords[1..]);

			PascalCase = upperWords[0] + secondAfterUpperWords;
			CamelCase = lowerWords[0] + secondAfterUpperWords;
		}
	}

	#endregion

	#region Properties

	/// <summary>
	/// スネークケースを取得します。
	/// </summary>
	public string SnakeCase { get; }

	/// <summary>
	/// パスカルケースを取得します。
	/// </summary>
	public string PascalCase { get; }

	/// <summary>
	/// キャメルケースを取得します。
	/// </summary>
	public string CamelCase { get; }

	#endregion

	#region Methods

	/// <summary>
	/// 値を検証します。
	/// </summary>
	/// <param name="value">値</param>
	/// <param name="message">メッセージ</param>
	/// <returns>検証に成功した場合は、<c>true</c>。それ以外の場合は、<c>false</c>。</returns>
	public static bool Validate(string value, out string message)
	{
		const string pattern = "^[0-9_a-z]+$";
		bool result = Regex.IsMatch(value, pattern);

		if (result)
		{
			message = string.Empty;
		}
		else
		{
			message = "列名は、スネークケースで入力してください。";
		}

		return result;
	}

	#endregion
}
