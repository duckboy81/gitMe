//Jean-Luc Duckworth Nov 2013
//Really trying to get git to get working
//Really really


#include<windows.h> 
#include<stdio.h> 


LRESULT CALLBACK TestWndProc(HWND, UINT, UINT, LONG);
BOOL Start_Prog(char *name);
HINSTANCE hInst;
UINT MSG_MMTTY = ::RegisterWindowMessage("MMTTY");
HWND MMTTY_Handle = NULL;
DWORD MMTTY_ThreadId;

HWND hWnd_main;

const enum {
	TXM_HANDLE = 0x8000, // MMTTY -> APP 
	TXM_REQHANDLE,
	TXM_START,
	TXM_CHAR,
	TXM_PTTEVENT,
};

const enum {
	RXM_HANDLE = 0x0000,	// APP -> MMTTY
	RXM_REQHANDLE,
	RXM_EXIT,
	RXM_PTT,
	RXM_CHAR,

	RXM_WINPOS,
	RXM_WIDTH,
	RXM_REQPARA,
	RXM_SETBAUD,
	RXM_SETMARK,

	RXM_SETSPACE,
	RXM_SETSWITCH,
	RXM_SETHAM,
	RXM_SHOWSETUP,
	RXM_SETVIEW,

	RXM_SETSQLVL,
	RXM_SHOW,
	RXM_SETFIG,
	RXM_SETRESO,
	RXM_SETLPF,

	RXM_SETTXDELAY,
	RXM_UPDATECOM,
	RXM_SUSPEND,
	RXM_NOTCH,
	RXM_PROFILE,

	RXM_TIMER,
	RXM_ENBFOCUS,
	RXM_SETDEFFREQ,
	RXM_SETLENGTH,
};

//--------------------------------------------------------------------------- 
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR szCommandLine, int nCmdShow)
{
	//HWND hWnd;
	WNDCLASS WndClass;
	MSG Msg;

	char szClassName[] = "Test";
	hInst = hInstance;

	WndClass.style = CS_HREDRAW | CS_VREDRAW;
	WndClass.lpfnWndProc = TestWndProc;
	WndClass.cbClsExtra = 0;
	WndClass.cbWndExtra = 0;
	WndClass.hInstance = hInstance;
	WndClass.hIcon = LoadIcon(hInstance, "Test");
	WndClass.hCursor = LoadCursor(NULL, IDC_ARROW);
	WndClass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	WndClass.lpszMenuName = "Menu";
	WndClass.lpszClassName = szClassName;

	if (!RegisterClass(&WndClass))
	{
		MessageBox(NULL, "Cannot register class", "Error", MB_OK);
		return 0;
	}

	hWnd_main = CreateWindow(szClassName, "Test Program", WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
		CW_USEDEFAULT, NULL, NULL, hInstance, NULL);

	if (!hWnd_main)
	{
		MessageBox(NULL, "Cannot create window", "Error", MB_OK);
		return 0;
	}

	ShowWindow(hWnd_main, nCmdShow);
	UpdateWindow(hWnd_main);


	while (GetMessage(&Msg, NULL, 0, 0))
	{
		TranslateMessage(&Msg);
		DispatchMessage(&Msg);
	} //while()

	return Msg.wParam;
}


LRESULT CALLBACK TestWndProc(HWND hWnd, UINT Msg, UINT wParam, LONG lParam)
{
	
	DWORD hPrgWnd;
	HDC hDc;
	PAINTSTRUCT sPnt;

	char *szError = "error starting MMTTTY";

	switch (Msg)
	{

	//LRESULT CALLBACK TestWndProc(HWND hWnd, UINT Msg, UINT wParam, LONG lParam) { switch (Msg) { case MSG_MMTTY: switch (wParam) { case TXM_THREAD: break; } break; case WM_DESTROY: PostQuitMessage(0); return 0; } return DefWindowProc(hWnd, Msg, wParam, lParam); }

	case WM_CREATE:

		//if (!Start_Prog("C:\\Users\\C14JeanLuc.Duckworth\\Desktop\\School\\M1.Capstone\\MMTTY168A-i\\MMTTY.EXE -r -s -t -u -a -h"))
		if (!Start_Prog("C:\\Users\\C14JeanLuc.Duckworth\\Downloads\\MMTTY.EXE -r"))
		{
			hDc = BeginPaint(hWnd, &sPnt);
			TextOut(hDc, 10, 10, szError, strlen(szError));
			EndPaint(hWnd, &sPnt);
			return 0;
		}

		return 0;
	case WM_PAINT:
		return 0;

	case WM_DESTROY:
		::PostMessage(MMTTY_Handle, MSG_MMTTY, RXM_EXIT, NULL);
		PostQuitMessage(0);
		return 0;

	default:

		
		if (Msg == MSG_MMTTY)
		{

			switch (wParam)
			{

			case TXM_HANDLE:
				MMTTY_Handle = (HWND)lParam;
				hPrgWnd = (DWORD)lParam;
				::PostMessage(MMTTY_Handle, MSG_MMTTY, RXM_PTT, 2);
				::PostMessage(MMTTY_Handle, MSG_MMTTY, RXM_HANDLE, (LONG)hWnd);
				break;
			};
		}

	}

	return DefWindowProc(hWnd, Msg, wParam, lParam);
}


BOOL Start_Prog(char *name)
{
	STARTUPINFO         si;
	PROCESS_INFORMATION pi;



	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	si.wShowWindow = SW_NORMAL;
	si.dwFlags = STARTF_USESHOWWINDOW;
	si.lpTitle = "TEST";

	return CreateProcess(
		NULL,
		name,
		NULL,
		NULL,
		FALSE,
		NORMAL_PRIORITY_CLASS,
		NULL,
		NULL,
		&si,
		&pi);

}