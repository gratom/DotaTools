using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Constants", fileName = "Constants", order = 0)]
public class Constants : ScriptableObject
{
    public TextAsset heroes;
    public List<DotaHero> heroesList;

    [ContextMenu("Parse")]
    public void Parse()
    {
        heroesList = ConstantParser.ParseHeroes(heroes.text).Values.ToList();
    }
}