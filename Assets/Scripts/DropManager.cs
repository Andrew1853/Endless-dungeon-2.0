using DemiurgEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    private void Start()
    {
        EventBus.Subscribe<CharacterDeathEvent>(OnCharacterDeath);
    }
    void OnCharacterDeath(CharacterDeathEvent data)
    {

    }
}