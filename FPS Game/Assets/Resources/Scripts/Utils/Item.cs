using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public ItemInfo itemInfo;
	public GameObject itemGameObject;

	public abstract float Use();
	public abstract float RemainingCooldown();
}