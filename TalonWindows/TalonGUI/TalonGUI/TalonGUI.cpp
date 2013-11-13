// TalonGUI.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "TalonGUI.h"

#define MAX_LOADSTRING 100

BOOL Start_Prog(char *name);

//Duck Global Variables:
UINT MSG_MMTTY = ::RegisterWindowMessage((LPCWSTR)"MMTTY");
HWND MMTTY_Handle = NULL;
DWORD MMTTY_ThreadId;

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

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);

int APIENTRY _tWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPTSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

 	// TODO: Place code here.
	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_TALONGUI, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_TALONGUI));

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_TALONGUI));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_TALONGUI);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   HWND hWnd;

   char *szError = "error starting MMTTTY";
   PAINTSTRUCT ps;
   HDC hdc;
   hInst = hInstance; // Store instance handle in our global variable

   hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

   if (!hWnd)
   {
      return FALSE;
   }

   ShowWindow(hWnd, nCmdShow);
   UpdateWindow(hWnd);

   if (!Start_Prog("C:\\Users\\C14JeanLuc.Duckworth\\Downloads\\MMTTY.EXE -r"))
   {
	   hdc = BeginPaint(hWnd, &ps);
	   TextOut(hdc, 10, 10, (LPCWSTR)szError, strlen(szError));
	   EndPaint(hWnd, &ps);
	   return 0;
   }

   return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	DWORD hPrgWnd;
	PAINTSTRUCT ps;
	HDC hdc;
	
	TCHAR greeting[] = _T("Hello, World!");
	char *szError = "error starting MMTTTY";

	switch (message)
	{
	case WM_CREATE:

		//if (!Start_Prog("C:\\Users\\C14JeanLuc.Duckworth\\Desktop\\School\\M1.Capstone\\MMTTY168A-i\\MMTTY.EXE -r -s -t -u -a -h"))
		//if (!Start_Prog("C:\\Users\\C14JeanLuc.Duckworth\\Downloads\\MMTTY.EXE -r"))
		//{
		//	hdc = BeginPaint(hWnd, &ps);
		//	TextOut(hdc, 10, 10, (LPCWSTR)szError, strlen(szError));
		//	EndPaint(hWnd, &ps);
		//	return 0;
		//}
		return 0;

	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		// TODO: Add any drawing code here...
		// Here your application is laid out.
			// For this introduction, we just print out "Hello, World!"
			// in the top left corner.

			TextOut(hdc,
			5, 5,
			greeting, _tcslen(greeting));
		// End application-specific layout section.
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		if (message == MSG_MMTTY)
		{

			switch (wParam)
			{

			case TXM_HANDLE:
				MMTTY_Handle = (HWND)lParam;
				hPrgWnd = (DWORD)lParam;
				::PostMessage(MMTTY_Handle, MSG_MMTTY, RXM_PTT, 2);
				::PostMessage(MMTTY_Handle, MSG_MMTTY, RXM_HANDLE, (LONG)hWnd);
				break;

			case TXM_CHAR:
				break;
			}; //switch(wParam)
		} //if(MSG_MMTTY)

		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}


BOOL Start_Prog(char *name)
{
	STARTUPINFO         si;
	PROCESS_INFORMATION pi;



	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	si.wShowWindow = SW_NORMAL;
	si.dwFlags = STARTF_USESHOWWINDOW;
	si.lpTitle = (LPWSTR)"TEST";

	return CreateProcess(
		NULL,
		(LPWSTR)name,
		NULL,
		NULL,
		FALSE,
		NORMAL_PRIORITY_CLASS,
		NULL,
		NULL,
		&si,
		&pi);

}