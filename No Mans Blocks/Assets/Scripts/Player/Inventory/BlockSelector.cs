using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Voxelated;
using Voxelated.Terrain;

public enum SelectorMode { Hidden = 0, Fixed = 1, Dig = 2, BuildCube = 3, BuildPillar = 4}
/// <summary>
/// Selector Handler
/// 
/// Controls when and how to display selector for player. Also controls what it should
/// intersect and what it should ignore. It will handle instantiating and deleting the 
/// active selector.
/// </summary>
public class BlockSelector : MonoBehaviour {
    private static readonly Vector3 PillarSize = new Vector3(1.0f, 3.0f, 1.0f);
    private static readonly Vector3 CubeSize = new Vector3(1.0f, 1.0f, 1.0f);
    private static readonly Vector3 HiddenPos = new Vector3(-77.0f, -77.0f, -77.0f);


    public Camera playerCamera;
    public GameObject selectorPrefab;
   // private WorldFragment worldFragment;
    private GameObject selectorGO;
    //private MeshRenderer  selectorRenderer;
    private Selector selector;
    private RaycastHit hit;
    private SelectorMode mode;

    public static Vector3 Size { get; set; }
    public static float ReachLength { get; set; }
    public GameObject SelectorGameObject { get { return selectorGO; } }
  //  public WorldFragment WorldFragment { get { return worldFragment; } }

    /// <summary>
    /// Returns the block that was hit provided the selector isn't in it's hiding spot.
    /// </summary>
    public RaycastHit? BlockHit {
        get {
            if (selector == null) {
                return null;
            }

            if (selector.Position.Equals(HiddenPos)) {
                return null;
            }
            else {
                return hit;
            }
        }
    }

    /// <summary>
    /// Set the SelectorMode. If its for building pillars we need to set size accordingly.
    /// </summary>
    public SelectorMode Mode {
        get {
            return mode;
        }
        set {
            mode = value;
            Size = (int)mode == 4 ? PillarSize : CubeSize;
        }
    }

    //I don't like defining values above. MY CODE, MY RULES!
    void Awake() {
        mode = SelectorMode.Hidden;
        Size = CubeSize;

        ReachLength = 5.0f;
    }

    void Start() {
        //It can change depending on if we are a regular player or map editor player
        if (playerCamera == null) {
            playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        }

        //Get the prefab for modify.cs
        if (mode == SelectorMode.Fixed) {
            CreateSelector(true);
        }
    }

    // Update is called once per frame
    void Update() {
        if (selectorGO != null) {
            selector.Position = DetermineSelectorLocation();
            selector.Rotation = DetermineSelectorRotation();

            //If not hidden. We want to render it.
            //selectorRenderer.enabled = (selector.Position != HiddenPos);

            //Rotate size to match if selector is a pillar I don't remember how this works
            if (mode == SelectorMode.BuildPillar) {
                Vector3 hitNorm = hit.normal;

                hitNorm.x = Mathf.Abs(hitNorm.x);
                hitNorm.y = Mathf.Abs(hitNorm.y);
                hitNorm.z = Mathf.Abs(hitNorm.z);

                Vector3 newToolSize = new Vector3(1.0f, 1.0f, 1.0f);
                newToolSize.x += (2 * hitNorm.x);
                newToolSize.y += (2 * hitNorm.y);
                newToolSize.z += (2 * hitNorm.z);

                Size = newToolSize;
            }
            //Check if we need to update size on screen.
            if (selector.Size != Size)
                selector.Size = Size;
        }
        //No selector exists. Create one.
        else
            CreateSelector();
    }

    /// <summary>
    /// Creates a new block selector instance.
    /// </summary>
    private void CreateSelector(bool overrideValidPos = false) {
        Vector3 selPos = DetermineSelectorLocation();

        selectorGO = (GameObject)Instantiate(selectorPrefab, selPos, Quaternion.identity, transform);
        selector = selectorGO.GetComponent<Selector>();
        //selectorRenderer = selectorGO.GetComponent<MeshRenderer>();

        if (mode == SelectorMode.Fixed)
           // worldFragment = selectorGO.GetComponentInChildren<WorldFragment>();

        //Set it to the correct size.
        selector.Size = Size;
    }

    /// <summary>
    /// Deterimes what the new rotation of the selector should be.
    /// </summary>
    /// <returns>The selector rotation.</returns>
    private Quaternion DetermineSelectorRotation() {
        //Only digTool can hit fragments. No need to rotate for blocks duh
        if (mode != SelectorMode.Dig)
            return Quaternion.identity;

        //If we hit a fragment return it's rotation.
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, ReachLength, WorldSettings.FragmentLayerMask))
            return hit.transform.rotation;
        else
            return Quaternion.identity;
    }

    /// <summary>
	/// Determines the selector location. If the buildMode is in pillar mode then it will place the selector
	/// so it extrudes out from the block face that is being looked at. If not it will center the selector around the block being focused on.
	/// </summary>
	private Vector3 DetermineSelectorLocation() {
        Vector3 selPos = HiddenPos;             //0

        switch (mode) {
            case SelectorMode.Fixed:            //1
                Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                Vector3 selectorPos = ray.GetPoint(ReachLength);
                selPos = new Vector3(Mathf.RoundToInt(selectorPos.x), Mathf.RoundToInt(selectorPos.y), Mathf.RoundToInt(selectorPos.z));
                break;

            case SelectorMode.Dig:              //2
            case SelectorMode.BuildCube:        //3
            case SelectorMode.BuildPillar:      //4
                int layerMask = ((int)mode != 2) ? WorldSettings.BlockLayerMask : WorldSettings.BlockAndFragMask;
                Transform castPoint = playerCamera.transform;

                //See if anything is hit.
                if (Physics.Raycast(castPoint.position, castPoint.forward, out hit, ReachLength, layerMask)) {

                    //We hit a fragment
                    if (hit.transform.gameObject.layer == WorldSettings.FragmentLayer) {
                        //Get the real world rounded position of the block in the fragment.
                        //selPos = TerrainEdit.GetFragmentBlockPos(hit);
                    }

                    //We hit a block
                    else {
                        bool adjacent = ((int)mode > 2);
                     //   selPos = TerrainEdit.GetBlockPos(hit, adjacent).ToVector3();

                        //Pillar build mode.
                        if (mode == SelectorMode.BuildPillar) {
                        //    Vector3Int norm = new Vector3Int(hit.normal);

                            //Fix position of selector 
                            //selPos.x += norm.x;
                            //selPos.y += norm.y;
                            //selPos.z += norm.z;
                        }
                    }
                }
                break;
        }
        return selPos;
    }

    /// <summary>
    /// Performs the tools raycast to see if it hits a block or not. Info can be retrieved 
    /// from the raycasthit.
    /// </summary>
    public bool CastForBlock(out RaycastHit hit) {
        int layerMask = ((int)mode != 2) ? WorldSettings.BlockLayerMask : WorldSettings.BlockAndFragMask;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, ReachLength, layerMask)) {
            return true;
        }
        return false;
    }
}
