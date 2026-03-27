using UnityEngine;

public class ForceInstancing : MonoBehaviour {

    void Awake() {

        // Vibe-coded
        GetComponent<Renderer>().SetPropertyBlock(new MaterialPropertyBlock());

    }
}