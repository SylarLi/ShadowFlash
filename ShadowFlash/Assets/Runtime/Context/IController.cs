public interface IController
{
	ModelType type { get; }

	ModelType[] dependTypes { get; }

	void InjectModel(IModel[] models);
}

