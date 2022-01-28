using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Weapon found on the ground
public abstract class WeaponDrop
{
    public PlayerController playerController;
	public string audioType;
	public int audioClipIndex = -1;
	//public abstract override float Use();

	protected float nextAttack;
	public bool enable = true;
	protected float cooldown = 0f;
	// public override float RemainingCooldown()
	// {
	// 	var current = Time.time;
	// 	var ratio = current / nextAttack;
	// 	return Mathf.Min(1, ratio) ;
	// }
	protected IEnumerator OnCooldown()
	{
		enable = false; 
		yield return new WaitForSeconds(cooldown);
		enable = true;
	}
}
