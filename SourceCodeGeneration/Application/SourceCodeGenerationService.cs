using System.Text;
using SourceCodeGeneration.Application.Command;

namespace SourceCodeGeneration.Application;

/// <summary>
/// ソースコード生成サービス
/// </summary>
public class SourceCodeGenerationService : ISourceCodeGenerationService
{
    #region Methods

    /// <summary>
    /// 不変オブジェクトを生成します。
    /// </summary>
    /// <param name="command">コマンド</param>
    /// <returns>生成したファイルのパスを返します。</returns>
    public string GenerateImmutableObject(ImmutableObjectGenerationCommand command)
    {
        var properties = command.Properties.Select(property => new PropertyTray(property));
        var firstProperty = properties.First();
        var secondAndSubsequentProperties = properties.Skip(1);

        using var writer = new StreamWriter(command.FilePath, append: false, Encoding.UTF8);

        // Namespace
        writer.Write("namespace ");
        writer.Write(command.NamespaceSequence);
        writer.WriteLine(";");

        writer.WriteLine();

        // Class - Documentation comment
        writer.WriteLine("/// <summary>");
        writer.Write("/// ");
        writer.WriteLine(command.ClassDescription);
        writer.WriteLine("/// </summary>");

        // Class - Begin: Definition
        writer.Write("public class ");
        writer.WriteLine(command.ClassName);
        writer.WriteLine("{");

        // Constructor
        {
            // Begin: Region derective
            writer.WriteLine("    #region Constructors");
            writer.WriteLine();

            // Documentation comment
            writer.WriteLine("    /// <summary>");
            writer.Write("    /// ");
            writer.Write(command.ClassDescription);
            writer.WriteLine("を初期化します。");
            writer.WriteLine("    /// </summary>");

            foreach (var property in properties)
            {
                writer.Write("    /// <param name=\"");
                writer.Write(property.ParameterName);
                writer.Write("\">");
                writer.Write(property.PropertyDescription);
                writer.WriteLine("</param>");
            }

            // Begin: Definition
            writer.Write("    public ");
            writer.Write(command.ClassName);
            writer.Write("(");

            var writeParameter = (StreamWriter writer, PropertyTray property) =>
            {
                writer.Write(property.TypeName);
                writer.Write(" ");
                writer.Write(property.ParameterName);
            };

            writeParameter(writer, firstProperty);
            foreach (var property in secondAndSubsequentProperties)
            {
                writer.Write(", ");
                writeParameter(writer, property);
            }

            writer.WriteLine(")");
            writer.WriteLine("    {");

            foreach (var property in properties)
            {
                writer.Write("        ");
                writer.Write(property.PropertyName);
                writer.Write(" = ");
                writer.Write(property.ParameterName);
                writer.WriteLine(";");
            }

            // End: Definition
            writer.WriteLine("    }");

            // End: Region derective
            writer.WriteLine();
            writer.WriteLine("    #endregion");
        }

        writer.WriteLine();

        // Property
        {
            // Begin: Region derective
            writer.WriteLine("    #region Properties");
            writer.WriteLine();

            var writeProperty = (StreamWriter writer, PropertyTray property) =>
            {
                // Documentation comment
                writer.WriteLine("    /// <summary>");
                writer.Write("    /// ");
                writer.Write(property.PropertyDescription);
                writer.WriteLine("を取得します。");
                writer.WriteLine("    /// </summary>");

                // Begin: Definition
                writer.Write("    public ");
                writer.Write(property.TypeName);
                writer.Write(" ");
                writer.WriteLine(property.PropertyName);
                writer.WriteLine("    {");
                writer.WriteLine("        get;");
                writer.WriteLine("    }");
            };

            writeProperty(writer, firstProperty);
            foreach (var property in secondAndSubsequentProperties)
            {
                writer.WriteLine();
                writeProperty(writer, property);
            }

            // End: Region derective
            writer.WriteLine();
            writer.WriteLine("    #endregion");
        }

        writer.WriteLine();

        // Method
        {
            // Begin: Region derective
            writer.WriteLine("    #region Methods");
            writer.WriteLine();

            // Documentation comment
            writer.WriteLine("    /// <summary>");
            writer.WriteLine("    /// 現在のオブジェクトを表す文字列を返します。");
            writer.WriteLine("    /// </summary>");
            writer.WriteLine("    /// <returns>現在のオブジェクトを表す文字列。</returns>");

            // Begin: Definition
            writer.Write("    public override string ToString() => $\"{nameof(");
            writer.Write(command.ClassName);
            writer.Write(")} {{ ");

            var writeProperty = (StreamWriter writer, PropertyTray property) =>
            {
                writer.Write("{nameof(");
                writer.Write(property.PropertyName);
                writer.Write(")} = {");
                writer.Write(property.PropertyName);
                writer.Write("}");
            };

            writeProperty(writer, firstProperty);
            foreach (var property in secondAndSubsequentProperties)
            {
                writer.Write(", ");
                writeProperty(writer, property);
            }

            // End: Definition
            writer.WriteLine(" }}\";");

            // End: Region derective
            writer.WriteLine();
            writer.WriteLine("    #endregion");
        }

        // Class - End: Definition
        writer.WriteLine("}");

        return command.FilePath;
    }

    /// <summary>
    /// 不変オブジェクトを生成します。
    /// </summary>
    /// <param name="command">コマンド</param>
    /// <returns>生成したファイルのパスを返します。</returns>
    public async Task<string> GenerateImmutableObjectAsync(ImmutableObjectGenerationCommand command)
    {
        return await Task.Run(() => GenerateImmutableObject(command));
    }

    #endregion

    #region Nested types

    /// <summary>
    /// プロパティトレイ
    /// </summary>
    private class PropertyTray
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
        public string PropertyName
        {
            get;
        }

        /// <summary>
        /// プロパティの説明を取得します。
        /// </summary>
        public string PropertyDescription
        {
            get;
        }

        /// <summary>
        /// 型名を取得します。
        /// </summary>
        public string TypeName
        {
            get;
        }

        /// <summary>
        /// パラメーター名を取得します。
        /// </summary>
        public string ParameterName
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>現在のオブジェクトを表す文字列。</returns>
        public override string ToString() => $"{nameof(PropertyTray)} {{ {nameof(PropertyName)} = {PropertyName}, {nameof(PropertyDescription)} = {PropertyDescription}, {nameof(TypeName)} = {TypeName}, {nameof(ParameterName)} = {ParameterName} }}";

        #endregion
    }

    #endregion
}
