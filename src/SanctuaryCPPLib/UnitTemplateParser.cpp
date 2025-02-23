#include "pch.h"
#include "UnitTemplateParser.h"
#include <iostream>

extern "C" __declspec(dllexport) int AddNumbers(int a, int b) {
	return a + b;
}


