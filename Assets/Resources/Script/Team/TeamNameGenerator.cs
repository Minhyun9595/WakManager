using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamNameGenerator
{
    public List<string> allCombination = new List<string>();
    public List<string> firstNameTemplate = new List<string>
    {
        "천양", "두칠", "두세븐", "왁타", "우왁굳", "티", "카오닝"
    };

    public List<string> nameTemplate = new List<string>
    {
        "다이노스", "일레븐", "바로셀로나", "스타즈", "다이노스",
        "아토믹핑크", "드래곤즈", "호드",
        "와이번스", "블랙맘바", "얼라이언스"
    };

    public TeamNameGenerator()
    {
        GenerateTeams();
    }

    public void GenerateTeams()
    {
        foreach (var firstName in firstNameTemplate)
        {
            foreach (var name in nameTemplate)
            {
                allCombination.Add($"{firstName} {name}");
            }
        }
    }

    public string GetRandomName()
    {
        // 중복되지 않는 이름을 랜덤으로 선택
        while (allCombination.Count > 0)
        {
            int index = Random.Range(0, allCombination.Count);
            var name = allCombination[index];
            allCombination.RemoveAt(index); // 선택된 이름은 제거
            return name;
        }

        return string.Empty;
    }
}
