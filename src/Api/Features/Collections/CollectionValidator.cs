using Poucno.Backend.Api.Features.Collections.Dtos;
using Poucno.Backend.Api.Features.Games;

namespace Poucno.Backend.Api.Features.Collections;

public static class CollectionValidator
{
    public const int MaxName = 80;
    public const int MaxDescription = 1000;
    public const int MaxQuestion = 200;
    public const int MaxHint = 200;
    public const int MaxAnswer = 40;
    public const int MinGrade = 1;
    public const int MaxGrade = 9;

    public static Dictionary<string, string[]> Validate(CollectionInput input, IEnumerable<GameOption> games)
    {
        var errors = new Dictionary<string, string[]>();

        var name = input.Name?.Trim() ?? "";
        if (name.Length == 0) errors["name"] = ["Name is required."];
        else if (name.Length > MaxName) errors["name"] = [$"Name can be at most {MaxName} characters."];

        if (string.IsNullOrWhiteSpace(input.Subject)) errors["subject"] = ["Subject is required."];

        if (input.Grade is < MinGrade or > MaxGrade)
            errors["grade"] = [$"Grade must be between {MinGrade} and {MaxGrade}."];

        if ((input.Description?.Trim().Length ?? 0) > MaxDescription)
            errors["description"] = [$"Description can be at most {MaxDescription} characters."];

        var game = games.FirstOrDefault(g => g.Id == input.GameId);
        if (game is null)
            errors["gameId"] = ["Unknown game."];

        var questions = input.Questions ?? [];
        for (var i = 0; i < questions.Count; i++)
            ValidateQuestion(questions[i], i, game, errors);

        return errors;
    }

    private static void ValidateQuestion(QuestionDto q, int index, GameOption? game, Dictionary<string, string[]> errors)
    {
        var key = $"questions[{index}]";

        var text = q.Question?.Trim() ?? "";
        if (text.Length == 0) errors[$"{key}.question"] = ["Question is required."];
        else if (text.Length > MaxQuestion) errors[$"{key}.question"] = [$"Question can be at most {MaxQuestion} characters."];

        if ((q.Hint?.Trim().Length ?? 0) > MaxHint)
            errors[$"{key}.hint"] = [$"Hint can be at most {MaxHint} characters."];

        var answers = (q.PossibleAnswers ?? []).Select(a => a?.Trim() ?? "").ToList();

        if (game is not null && (answers.Count < game.MinAnswers || answers.Count > game.MaxAnswers))
        {
            errors[$"{key}.possibleAnswers"] = [game.MaxAnswers == 0
                ? "This game takes no possible answers, only the answer."
                : $"This game needs {game.MinAnswers} to {game.MaxAnswers} answers."];
        }
        else if (answers.Any(a => a.Length == 0))
            errors[$"{key}.possibleAnswers"] = ["Answers cannot be empty."];
        else if (answers.Any(a => a.Length > MaxAnswer))
            errors[$"{key}.possibleAnswers"] = [$"Each answer can be at most {MaxAnswer} characters."];
        else if (answers.Distinct().Count() != answers.Count)
            errors[$"{key}.possibleAnswers"] = ["Answers must be different."];

        // Games without possible answers (e.g. word guessing) only carry the answer itself.
        var answer = q.Answer?.Trim() ?? "";
        if (answer.Length == 0)
            errors[$"{key}.answer"] = ["Answer is required."];
        else if (answer.Length > MaxAnswer)
            errors[$"{key}.answer"] = [$"Answer can be at most {MaxAnswer} characters."];
        else if (game?.MaxAnswers != 0 && !answers.Contains(answer))
            errors[$"{key}.answer"] = ["Answer must be one of the possible answers."];
    }
}
