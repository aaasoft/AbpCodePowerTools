using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AbpDtoGenerator.Base;

public abstract class BaseViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected void InvokePropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	protected T DClone<T>() where T : BaseViewModel
	{
		_ = (T)this;
		using MemoryStream memoryStream = new MemoryStream();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(memoryStream, this);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		T result = (T)binaryFormatter.Deserialize(memoryStream);
		memoryStream.Close();
		return result;
	}
}
