using UnityEngine;
using UnityEngine.SceneManagement;

public class HamsterBallSimulator : MonoBehaviour
{
	public GameObject controller;

	private Scene mainScene;
	private PhysicsScene mainPhysicsScene;

	private Scene hamsterSimulationScene;
	private PhysicsScene hamsterSimulationPhyicsScene;


	public static HamsterBallSimulator Get()
	{
		return FindObjectOfType<HamsterBallSimulator>();
	}

	public static PhysicsScene getPhysicsScene()
	{
		return Get().mainPhysicsScene;
	}

	public GameObject SpawnGameObject(GameObject obj, Vector3 position, Quaternion rotation)
	{
		GameObject spawned = Instantiate(obj, position, rotation);
		SceneManager.MoveGameObjectToScene(spawned, mainScene);

		return spawned;
	}

	public void AddGameObjectToMainPhysicsSim(GameObject obj)
	{
		SceneManager.MoveGameObjectToScene(obj, mainScene);
	}

	private void Awake()
	{
		Physics.autoSimulation = false;
		CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics3D);

		mainScene = SceneManager.CreateScene("World", csp);
		mainPhysicsScene = mainScene.GetPhysicsScene();

		foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
		{
			if (obj.scene.name == "Intro" || obj.scene.buildIndex == -1) {
				continue;
			}
			if (obj.transform.parent == null && obj != gameObject)
			{
				SceneManager.MoveGameObjectToScene(obj, mainScene);
			}
		}

		hamsterSimulationScene = SceneManager.CreateScene("HamsterSimScene", csp);
		hamsterSimulationPhyicsScene = hamsterSimulationScene.GetPhysicsScene();

		SceneManager.MoveGameObjectToScene(gameObject, hamsterSimulationScene);
	}

	private void OnDestroy()
	{
	}

	private void FixedUpdate()
	{
		mainPhysicsScene.Simulate(Time.fixedDeltaTime);
		transform.position = controller.transform.position;
		transform.rotation = controller.transform.rotation;
		hamsterSimulationPhyicsScene.Simulate(Time.fixedDeltaTime);
	}
}
