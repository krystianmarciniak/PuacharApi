namespace PucharApi.Domain;

public class Bracket
{
  public int Id { get; set; }

  public List<Match> Matches { get; set; } = new();

  // ⭐ Diagram → później mutacja GraphQL
  public void GenerateBracket(List<User> participants)
  {
    if (participants.Count < 2)
      throw new InvalidOperationException("Brak wystarczającej liczby uczestników.");

    Matches.Clear();

    int round = 1;
    for (int i = 0; i < participants.Count; i += 2)
    {
      var p1 = participants[i];
      User? p2 = (i + 1 < participants.Count) ? participants[i + 1] : null;

      var match = new Match
      {
        Round = round,
        Player1 = p1,
        Player2 = p2,
        Winner = p2 is null ? p1 : null
      };

      Matches.Add(match);
    }
  }

  public List<Match> GetMatchesForRound(int round)
      => Matches.Where(m => m.Round == round).ToList();
}
