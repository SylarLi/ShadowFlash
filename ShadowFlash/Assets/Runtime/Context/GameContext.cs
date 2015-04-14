using UnityEngine;
using System.Collections.Generic;

public class GameContext : MonoBehaviour, IGameContext
{
	private IDictionary<ModelType, IModel> models;

	private IList<IController> controllers;

	private void Awake()
	{
		models = new Dictionary<ModelType, IModel>();
		controllers = new List<IController>();

		Object.DontDestroyOnLoad(GameObject.Find("GameContext"));
		Object.DontDestroyOnLoad(GameObject.Find("Main Camera"));
		Object.DontDestroyOnLoad(GameObject.Find("LoadProxy"));
	}

	private void Start()
	{
		InitModel();
		InitController();
		Inject();
		Run();
	}
	
	public void InitModel()
	{
		models.Add(ModelType.Scene, new SceneModel());
	}

	public void InitController()
	{
		controllers.Add(new SceneController());
	}
	
	public void Inject()
	{
		List<ModelType> depends = new List<ModelType>();
		foreach (IController controller in controllers)
		{
			depends.Clear();
			depends.Add(controller.type);
			if (controller.dependTypes != null)
			{
				depends.AddRange(controller.dependTypes);
			}
			controller.InjectModel(depends.ConvertAll<IModel>((ModelType type) => models[type]).ToArray());
		}
	}

	public void Run()
	{
		(models[ModelType.Scene] as SceneModel).mapId = 1;
	}
}