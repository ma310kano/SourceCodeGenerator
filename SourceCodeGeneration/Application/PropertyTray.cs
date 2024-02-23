using SourceCodeGeneration.Application.Command;

namespace SourceCodeGeneration.Application;

/// <summary>
/// プロパティトレイ
/// </summary>
internal class PropertyTray
{
	#region Constructors

	/// <summary>
	/// プロパティトレイを初期化します。
	/// </summary>
	/// <param name="command">コマンド</param>
	public PropertyTray(PropertyCommand command)
	{
		PropertyName = command.PropertyName;
		PropertyDescription = command.PropertyDescription;
		TypeName = command.TypeName;
		ParameterName = command.PropertyName[0..1].ToLower() + command.PropertyName[1..];
	}

	#endregion

	#region Properties

	/// <summary>
	/// プロパティ名を取得します。
	/// </summary>
	public string PropertyName { get; }

	/// <summary>
	/// プロパティの説明を取得します。
	/// </summary>
	public string PropertyDescription { get; }

	/// <summary>
	/// 型名を取得します。
	/// </summary>
	public string TypeName { get; }

	/// <summary>
	/// パラメーター名を取得します。
	/// </summary>
	public string ParameterName { get; }

	#endregion

	#region Methods

	/// <summary>
	/// 現在のオブジェクトを表す文字列を返します。
	/// </summary>
	/// <returns>現在のオブジェクトを表す文字列。</returns>
	public override string ToString() => $"{nameof(PropertyTray)} {{ {nameof(PropertyName)} = {PropertyName}, {nameof(PropertyDescription)} = {PropertyDescription}, {nameof(TypeName)} = {TypeName}, {nameof(ParameterName)} = {ParameterName} }}";

	#endregion
}
