using SourceCodeGeneration.Application.Command;

namespace SourceCodeGeneration.Application;

/// <summary>
/// プロパティトレイ
/// </summary>
/// <param name="command">コマンド</param>
internal class PropertyTray(EntityPropertyCommand command)
{
	#region Properties

	/// <summary>
	/// プロパティ名を取得します。
	/// </summary>
	public string PropertyName { get; } = command.PropertyName;

	/// <summary>
	/// プロパティの説明を取得します。
	/// </summary>
	public string PropertyDescription { get; } = command.PropertyDescription;

	/// <summary>
	/// 型名を取得します。
	/// </summary>
	public string TypeName { get; } = command.TypeName;

	/// <summary>
	/// 主キーかどうかを取得します。
	/// </summary>
	public bool IsPrimaryKey { get; } = command.IsPrimaryKey;

	/// <summary>
	/// パラメーター名を取得します。
	/// </summary>
	public string ParameterName { get; } = command.PropertyName[0..1].ToLower() + command.PropertyName[1..];

	#endregion

	#region Methods

	/// <summary>
	/// 現在のオブジェクトを表す文字列を返します。
	/// </summary>
	/// <returns>現在のオブジェクトを表す文字列。</returns>
	public override string ToString() => $"{nameof(PropertyTray)} {{ {nameof(PropertyName)} = {PropertyName}, {nameof(PropertyDescription)} = {PropertyDescription}, {nameof(TypeName)} = {TypeName}, {nameof(ParameterName)} = {ParameterName} }}";

	#endregion
}
