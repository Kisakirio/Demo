using System;
using TMPro;
using UnityEngine;

    public class HPValue:MonoBehaviour
    {
	    [SerializeField]
	    private TextMeshProUGUI tmp;

	    [SerializeField]
	    private HPBar hp;

	    private int _hp;

	    private void Update()
	    {
		    if (Mathf.Round(hp.health) != _hp)
		    {
			    tmp.text = ((int)hp.health).ToString();
			    _hp= (int)hp.health;
		    }
	    }

    }

