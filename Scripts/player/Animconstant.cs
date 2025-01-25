

using System.Collections.Generic;
using UnityEngine;

public class Animconstant
{

	public readonly string stand = "stand";

	public readonly string Run = "run";

	public readonly string playerDrop = "Player_Drop";

	public readonly string playerland = "Player_land";

	public readonly string playerDoubleJump = "Player_DoubleJump";

	public readonly string playerJump = "Player_Jump";

	public readonly string attack1 = "attack1";

	public readonly string playerattack2 = "Player_Attack2";

	public readonly string playerattack3 = "Player_Attack3";

	public readonly string playerattack4 = "Player_Attack4";

	public readonly string playerattack4_strong = "Player_Attack4_Strong";

	public readonly string playerattack_upwardAttack = "Player_UpwardAttack";

	public readonly string playerDash = "Player_Dash";

	public readonly string playerSlide = "Player_Slide";

	public readonly string playerAirAttack1 = "Player_AirAttack1";

	public readonly string playerAirAttack2 = "Player_AirAttack2";

	public readonly string playerAirattack3 = "Player_AirAttack3";

	public readonly string playerDeath = "Player_Death";

	public readonly string hurt = "hurt";

	public readonly string attack1_loop = "attack1_loop";

	public readonly string attack1_over = "attack1_over";

	public readonly string jump = "jump";

	public readonly string fall = "fall";



	private Animconstant(){}


	public static Animconstant _instance=new Animconstant();


}
