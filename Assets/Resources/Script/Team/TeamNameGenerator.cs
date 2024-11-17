using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamNameGenerator
{
    public List<string> allCombination = new List<string>();
    public List<string> firstNameTemplate = new List<string>
    {
        "õ��", "��ĥ", "�μ���", "��Ÿ", "��α�", "Ƽ", "ī����"
    };

    public List<string> nameTemplate = new List<string>
    {
        "���̳뽺", "�Ϸ���", "�ٷμ��γ�", "��Ÿ��", "������� ���̳뽺",
        "�������ũ", "���� �巡����", "�ݹݽ���ġŲ �Ϸ���",
        "������� ���̹���", "�쵿�縮", "���� ������", "�Ϸ��� ��Ʈ"
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
        // �ߺ����� �ʴ� �̸��� �������� ����
        while (allCombination.Count > 0)
        {
            int index = Random.Range(0, allCombination.Count);
            var name = allCombination[index];
            allCombination.RemoveAt(index); // ���õ� �̸��� ����
            return name;
        }

        return string.Empty;
    }
}
