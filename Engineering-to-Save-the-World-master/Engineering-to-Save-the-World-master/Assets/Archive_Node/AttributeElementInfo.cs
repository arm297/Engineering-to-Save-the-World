using System.Collections;
using System;

[Serializable]
public class AttributeElementInfo {

	public AttributeInfo parentAttribute;
	public int minimumValue, maximumValue, actualValue;

	public AttributeElementInfo(AttributeInfo _parentAttribute, int _minimumValue, int _actualValue, int _maximumValue) {
		parentAttribute = _parentAttribute;
		minimumValue = _minimumValue;
		maximumValue = _maximumValue;
		actualValue = _actualValue;
	}
}
