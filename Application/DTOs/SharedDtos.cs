namespace SurveyApiSystem.Application.DTOs;

public record CreateSurveyRequest(string Title, string Description, List<CreateQuestionRequest> Questions);
public record CreateQuestionRequest(string Text, List<string> Options);
public record VoteRequest(Guid QuestionId, Guid OptionId);
public record VoteResultDto(Guid QuestionId, Guid OptionId, int Count);
public record SurveyReportDto(Guid SurveyId, string Title, List<QuestionReportDto> Questions);
public record QuestionReportDto(Guid Id, string Text, List<OptionReportDto> Options);
public record OptionReportDto(Guid Id, string Text, int VoteCount);