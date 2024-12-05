using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface UseMoveAction
{
    [HideInInspector]
    List<MoveActionWorker> action_list { get; set; }
}
