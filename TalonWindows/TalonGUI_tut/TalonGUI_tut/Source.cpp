#include <windows.h>

LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance,
	LPSTR szCmdLine, int nShowCmd)
{
	static char name[] = "My Application";
	WNDCLASSEX wc;
	HWND hwnd;
	MSG Msg;

	//Step 1: Registering the Window Class
	wc.cbSize = sizeof(WNDCLASSEX);
	wc.style = CS_HREDRAW | CS_VREDRAW;
	wc.lpfnWndProc = WndProc;
	wc.cbClsExtra = 0;
	wc.cbWndExtra = 0;
	wc.hInstance = hInstance;
	wc.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
	wc.lpszMenuName = NULL;
	wc.lpszClassName = name;
	wc.hIconSm = LoadIcon(NULL, IDI_APPLICATION);

	if (!RegisterClassEx(&wc))
	{
		MessageBox(NULL, "Window Registration Failed!",
			"Registration Failure",
			MB_ICONEXCLAMATION | MB_OK);
		return 0;
	}

	// Step 2: Creating the Window
	hwnd = CreateWindowEx(
		WS_EX_CLIENTEDGE,
		name,
		"Talon Rock Monitor",
		WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, CW_USEDEFAULT, 400, 400,
		NULL, NULL, hInstance, NULL);

	ShowWindow(hwnd, nShowCmd);
	UpdateWindow(hwnd);

	// Step 3: The Message Loop
	while (GetMessage(&Msg, NULL, 0, 0) > 0)
	{
		TranslateMessage(&Msg);
		DispatchMessage(&Msg);
	}
	return (int)Msg.wParam;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg,
	WPARAM wParam, LPARAM lParam)
{
	HDC hdc;
	HPEN hPen;
	HBRUSH hBrush;
	PAINTSTRUCT ps;
	RECT rect;

	switch (msg)
	{
	case WM_PAINT:

		hdc = BeginPaint(hwnd, &ps);//Begin Painting
		hPen = CreatePen(BS_SOLID, 2, RGB(255, 0, 0));//Defined HPEN hPen
		hBrush = CreateSolidBrush(RGB(0, 255, 0));//Defined HBRUSH hBrush


		SelectObject(hdc, hPen);//Load the pen into the DC
		SelectObject(hdc, hBrush);//Load the brush into the DC

		Rectangle(hdc, 50, 50, 200, 100);//Make a rectangle, will use the brush and the pen

		MoveToEx(hdc, 50, 100, NULL);//Set beginning point at (50,100)
		LineTo(hdc, 70, 60);//Make a line from (50, 100) to (70, 60)
		LineTo(hdc, 90, 85);//Make a line from (70, 60) to (90, 85)
		LineTo(hdc, 110, 55);//Make a line from (90, 85) to (110, 55)
		LineTo(hdc, 120, 60);//Make a line from (110, 55) to (120, 60)
		LineTo(hdc, 140, 95);//Make a line from (120, 60) to (140, 95)
		LineTo(hdc, 180, 53);//Make a line from (140, 95 to (180, 53)

		SetTextColor(hdc, RGB(0, 0, 255));//Set the Text color
		TextOut(hdc, 95, 110, "A Graph", 7);//Draw the text


		GetClientRect(hwnd, &rect);
		Rectangle(hdc, rect.right / 2 - 50, rect.bottom / 2 - 20, rect.right / 2 + 50, rect.bottom / 2 + 20);

		DrawText(hdc, "Hello World!", -1, &rect, DT_SINGLELINE | DT_CENTER | DT_VCENTER);

		EndPaint(hwnd, &ps);
		break;

	case WM_CLOSE:
		DestroyWindow(hwnd);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	}
	return DefWindowProc(hwnd, msg, wParam, lParam);
}
