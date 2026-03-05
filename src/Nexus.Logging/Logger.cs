using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Nexus.Logging;

public abstract class Logger : ILogger
{
    #region Constructor

    public Logger(LogLevel minLogLvl = InitialMinimumLogLevel)
    {
        MinimumLogLevel = minLogLvl;
        IsLabelsEnabled = true;
        LabelFormat = DefaultLabelFormat;
    }

    #endregion

    #region Constants

    const LogLevel InitialMinimumLogLevel = LogLevel.None;

    const string DefaultLabelFormat = "[{LogLevel} | {Timestamp:MM-dd HH:mm:ss}]:";

    const string LogLevelPlaceholder = "LogLevel";
    const string LevelPlaceholder = "Level";
    const string TimestampPlaceholder = "Timestamp";

    const string NullLiteral = "null";
    const char OpenBrace = '{';
    const char CloseBrace = '}';
    const char FormatSeparator = ':';

    #endregion

    #region ILogger Properties

    public bool IsEnabled() => MinimumLogLevel > LogLevel.None;

    public void SetIsEnabled(bool value)
    {
        if (!value) MinimumLogLevel = LogLevel.None;
    }

    public LogLevel MinimumLogLevel { get; set; }
    public bool IsLabelsEnabled { get; set; }
    public bool IsSeparatorEnabled { get; set; }
    public string? LabelFormat { get; set; }

    #endregion

    #region ILogger Methods

    public void SetFormat(string format)
    {
        LabelFormat = format;
    }

    public virtual string Serialize()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        var obj = new
        {
            IsEnabled = IsEnabled(),
            MinimumLogLevel,
            IsLabelsEnabled,
            IsSeparatorEnabled,
            LabelFormat
        };

        return JsonSerializer.Serialize(obj, options);
    }

    public virtual void Deserialize(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.TryGetProperty("IsEnabled", out var isEnabled))
            SetIsEnabled(isEnabled.GetBoolean());

        if (root.TryGetProperty(nameof(MinimumLogLevel), out var minLevel))
            if (Enum.TryParse<LogLevel>(minLevel.GetString(), out var parsed))
                MinimumLogLevel = parsed;

        if (root.TryGetProperty(nameof(IsLabelsEnabled), out var isLabelsEnabled))
            IsLabelsEnabled = isLabelsEnabled.GetBoolean();

        if (root.TryGetProperty(nameof(IsSeparatorEnabled), out var isSeparatorEnabled))
            IsSeparatorEnabled = isSeparatorEnabled.GetBoolean();

        if (root.TryGetProperty(nameof(LabelFormat), out var labelFormat))
            LabelFormat = labelFormat.ValueKind == JsonValueKind.Null ? null : labelFormat.GetString();
    }

    public void Verbose(string msg, params object[]? props)
    {
        if (IsEnabled() && LogLevel.Verbose >= MinimumLogLevel)
            Log(LogLevel.Verbose, msg, props);
    }

    public void Debug(string msg, params object[]? props)
    {
        if (IsEnabled() && LogLevel.Debug >= MinimumLogLevel)
            Log(LogLevel.Debug, msg, props);
    }

    public void Info(string msg, params object[]? props)
    {
        if (IsEnabled() && LogLevel.Info >= MinimumLogLevel)
            Log(LogLevel.Info, msg, props);
    }

    public void Warning(string msg, params object[]? props)
    {
        if (IsEnabled() && LogLevel.Warning >= MinimumLogLevel)
            Log(LogLevel.Warning, msg, props);
    }

    public void Error(string msg, params object[]? props)
    {
        if (IsEnabled() && LogLevel.Error >= MinimumLogLevel)
            Log(LogLevel.Error, msg, props);
    }

    public void Error(Exception ex, string msg, params object[]? props)
    {
        if (IsEnabled() && LogLevel.Error >= MinimumLogLevel)
            Log(LogLevel.Error, ex, msg, props);
    }

    public void Critical(string msg, params object[]? props)
    {
        if (IsEnabled() && LogLevel.Critical >= MinimumLogLevel)
            Log(LogLevel.Critical, msg, props);
    }

    public void Critical(Exception ex, string msg, params object[]? props)
    {
        if (IsEnabled() && LogLevel.Critical >= MinimumLogLevel)
            Log(LogLevel.Critical, ex, msg, props);
    }

    #endregion

    #region Protected Methods

    protected abstract void OutputFormattedMessage(string msg);

    protected string InsertProperties(string msg, params object[]? props)
    {
        if (props == null || props.Length == 0)
            return msg;

        var bound = new List<(string name, int propIndex)>();
        var sb = new StringBuilder();
        var propIndex = 0;

        for (var i = 0; i < msg.Length; i++)
        {
            if (msg[i] != OpenBrace)
            {
                sb.Append(msg[i]);
                continue;
            }

            var closingBrace = msg.IndexOf(CloseBrace, i + 1);

            if (closingBrace == -1)
            {
                sb.Append(msg[i]);
                continue;
            }

            var tagStart = i + 1;
            var tagName = msg.Substring(tagStart, closingBrace - tagStart);
            i = closingBrace;

            var foundIndex = -1;

            for (var j = 0; j < bound.Count; j++)
                if (bound[j].name == tagName)
                {
                    foundIndex = bound[j].propIndex;
                    break;
                }

            if (foundIndex >= 0 && foundIndex < props.Length)
            {
                var prop = props[foundIndex];
                sb.Append(prop?.ToString() ?? NullLiteral);
            }
            else if (propIndex < props.Length)
            {
                var prop = props[propIndex];
                bound.Add((tagName, propIndex));
                propIndex++;
                sb.Append(prop?.ToString() ?? NullLiteral);
            }
            else
            {
                sb.Append(OpenBrace).Append(tagName).Append(CloseBrace);
            }
        }

        return sb.ToString();
    }

    protected string FormatLabel(string labelFormat, LogLevel level, DateTime timestamp)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < labelFormat.Length; i++)
        {
            if (labelFormat[i] != OpenBrace)
            {
                sb.Append(labelFormat[i]);
                continue;
            }

            var closingBrace = labelFormat.IndexOf(CloseBrace, i + 1);

            if (closingBrace == -1)
            {
                sb.Append(labelFormat[i]);
                continue;
            }

            var tagStart = i + 1;
            var tagLength = closingBrace - tagStart;
            var tagSpan = labelFormat.AsSpan(tagStart, tagLength);

            var colonIndex = tagSpan.IndexOf(FormatSeparator);
            ReadOnlySpan<char> tagName;
            ReadOnlySpan<char> format;

            if (colonIndex >= 0)
            {
                tagName = tagSpan.Slice(0, colonIndex);
                format = tagSpan.Slice(colonIndex + 1);
            }
            else
            {
                tagName = tagSpan;
                format = default;
            }

            i = closingBrace;

            if (tagName.SequenceEqual(LogLevelPlaceholder.AsSpan()) ||
                tagName.SequenceEqual(LevelPlaceholder.AsSpan()))
            {
                sb.Append(level.ToString());
            }
            else if (tagName.SequenceEqual(TimestampPlaceholder.AsSpan()))
            {
                if (!format.IsEmpty)
                    try
                    {
                        sb.Append(timestamp.ToString(format.ToString()));
                    }
                    catch (FormatException)
                    {
                        sb.Append(timestamp.ToString());
                    }
                else
                    sb.Append(timestamp.ToString());
            }
            else
            {
                sb.Append(OpenBrace).Append(tagSpan.ToString()).Append(CloseBrace);
            }
        }

        return sb.ToString();
    }

    #endregion

    #region Private Methods

    void Log(LogLevel logLvl, Exception ex, string msg, params object[]? props)
    {
        if (!IsEnabled() || logLvl < MinimumLogLevel)
            return;

        msg = $"{msg}\n{ex.Message}\n{ex.StackTrace}";
        Log(logLvl, msg, props);
    }

    void Log(LogLevel logLvl, string msg, params object[]? props)
    {
        if (!IsEnabled() || logLvl < MinimumLogLevel)
            return;

        var messageWithProps = InsertProperties(msg, props);

        string finalOutput;

        if (IsLabelsEnabled && !string.IsNullOrEmpty(LabelFormat))
        {
            var label = FormatLabel(LabelFormat, logLvl, DateTime.Now);
            finalOutput = $"{label} {messageWithProps}";
        }
        else
        {
            finalOutput = messageWithProps;
        }

        if (IsSeparatorEnabled)
            finalOutput += "\n";

        OutputFormattedMessage(finalOutput);
    }

    #endregion
}
