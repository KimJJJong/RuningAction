using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager instance { get; private set; }

    private Dictionary<ObstacleType, float> obstacle_damage_dic = new Dictionary<ObstacleType, float>(); 

    public enum ObstacleType
    {
        jump_obstacle_short,
        jump_obstacle_long,
        slide_obstacle,
        block_obstacle,
        defender_obstacle
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        //TODO: get a damage dictionary from server
        obstacle_damage_dic[ObstacleType.jump_obstacle_short] = 25;
        obstacle_damage_dic[ObstacleType.jump_obstacle_long] = 25;
        obstacle_damage_dic[ObstacleType.slide_obstacle] = 20;
        obstacle_damage_dic[ObstacleType.block_obstacle] = 30;
        obstacle_damage_dic[ObstacleType.defender_obstacle] = 10000;

    }

    public void HandleObstacleCollision(GameObject player, ObstacleBase obstacle)
    {
        float damage = obstacle_damage_dic[obstacle.obstacle_type];


        //TODO: Do the damage according to damage variable    
    }
}
