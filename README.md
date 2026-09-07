# 다크패턴 체험 미니게임 (Team 4)

Unity 6 기반의 **보이스피싱 · 다크패턴 인식 교육용 미니게임 모음**입니다.
가짜 체크박스, 도망가는 거절 버튼, 끝없는 약관 스크롤, 가짜 점검 팝업, OTP 입력 화면 등
실제 기만적 UI 패턴(Dark Pattern)을 직접 겪어보며 "왜 당하는지"를 체감하도록 설계했습니다.

플레이어는 제한 시간 **5분** 안에 5개 스테이지를 모두 통과해야 하고,
마지막에는 OTP 입력과 송금 팝업으로 이어지는 엔딩을 마주합니다.

---

## 실행 방법

| 항목 | 값 |
|---|---|
| Unity 버전 | **6000.3.15f1** (Unity 6) |
| 렌더 파이프라인 | URP 17.3.0 |
| 입력 시스템 | Input System 1.19.0 |
| 메인 씬 | `Assets/Scenes/TongHapTest.unity` |

1. Unity Hub에서 위 버전으로 프로젝트를 엽니다.
2. `Assets/Scenes/TongHapTest.unity` 씬을 로드합니다.
3. Play 버튼으로 실행합니다.

> 빌드는 Unity Editor의 Build Settings에서 수동으로 진행합니다. (CLI 빌드 스크립트 없음)

개별 미니게임만 확인하려면 `Assets/Scenes/` 아래의 단독 씬
(`Basketball.unity`, `BulletHell.unity`, `BeadCatcher.unity`, `CarShot.unity`, `Raycast.unity` 등)을 열면 됩니다.

---

## 게임 흐름

```
StageStartButton
      ↓
MiniGameSpawner.StartStage(n)
      ↓  MiniGamePool 에서 해당 스테이지 프리팹 랜덤 선택 → Instantiate
미니게임 인스턴스 (StageScreen 상속)
      ↓  MiniGameManager.NotifySuccess() / NotifyFail()
StageScreen.OnStageClearButtonClicked / OnGameOver
      ↓
MiniGameSpawner_Events → PopupGate → 성공 / 실패 / 점검 / 타임아웃 팝업
      ↓
StageClearManager.ClearStage(n) → AllCleared(5) 이면 OTP 엔딩
```

- 미니게임과 메인 시스템은 **전부 이벤트 기반**으로 통신합니다. 하드코딩된 크로스 의존성이 없습니다.
- `StageScreen.isFinished` 플래그로 성공/실패 중복 트리거를 방지합니다.
- 전역 `GameClockTimer`(5분)가 만료되면 일반 게임오버 대신
  `MiniGameSpawner.ForceGameOver()` → **가짜 "점검시간" 팝업**이 최우선으로 뜹니다.

---

## 주요 시스템

| 클래스 | 경로 | 역할 |
|---|---|---|
| `MiniGameManager` | `Scripts/MiniGameManager.cs` | 정적 이벤트 허브. `OnGame1~5Start`, `OnMiniGameSuccess`, `OnMiniGameFail` |
| `MiniGameSpawner` | `Scripts/MiniGameSpawner/` (partial 5개) | 스테이지 진행, 미니게임 스폰, 팝업 디스패치, OTP 엔딩 |
| `MiniGamePool` | `Scripts/MiniGameSpawner/MiniGamePool.cs` | 스테이지별 프리팹 배열. 중복 없이 무작위 선택, 소진 시 풀 리셋 |
| `MiniGameRuntimeBinder` | `Scripts/MiniGameSpawner/MiniGameRuntimeBinder.cs` | 리플렉션으로 `buttonHandler` 필드 자동 연결 및 `StartMiniGame()` 호출 |
| `PopupGate` / `ScreenRouter` | `Scripts/MiniGameSpawner/` | 팝업 가드·리셋, 타이틀/메인 화면 전환을 캡슐화 |
| `StageScreen` | `Scripts/StageScreen.cs` | 모든 미니게임의 베이스 클래스 |
| `StageClearManager` | `Scripts/StageClearManager.cs` | 클리어 스테이지 추적(`HashSet<int>`), `AllCleared()` |
| `GameClockTimer` | `Scripts/GameClockTimer.cs` | 전역 5분 카운트다운 + 시계 바늘 회전. `unscaledDeltaTime` 사용 |
| `MiniGamePopup` | `Scripts/Popup/` (partial 4개) | 성공·실패·점검·타임아웃·의미불명 팝업. `IsMaintenanceOpen`이 최우선 플래그 |
| `AudioManager` | `Scripts/AudioManager.cs` | BGM/SFX 제어. 뮤트 상태를 `PlayerPrefs`에 저장 |
| `JsonLoader` | `Scripts/JsonLoader.cs` | `Resources/GameTexts.json`에서 약관·개인정보처리방침 텍스트 로드 |

---

## 수록 미니게임

**다크패턴 계열** (`Assets/Scripts/MiniGame/`)

| 스크립트 | 패턴 |
|---|---|
| `CheckboxAgreeMiniGame` | 체크박스 10개 전체 동의 강요 + 거절 버튼이 커서를 피해 도망 |
| `ScrollAgreeMiniGame` | 끝까지 내려야만 활성화되는 초장문 약관 스크롤 |
| `HiddenAgreeGame` | 반투명하게 숨겨진 동의 버튼이 랜덤 시점에 등장 |
| `JumpButtonGame` | 동의 버튼이 포물선으로 튀어 다님 |
| `MovingMiniGame` | 동의 버튼이 계속 움직임 |
| `WallButtonGame` | 12×10 버튼 격자 속에 섞인 진짜 동의 버튼 찾기 |
| `ErrorClickGame` | 수십 개 에러 팝업 더미 속 진짜 팝업 찾기 |
| `ButtonMashMiniGame` / `ButtonSwapMashGame` | 게이지를 채우는 연타, 도중에 버튼 위치가 뒤바뀜 |
| `OTPMiniGame` | 엔딩 OTP 입력 화면 |

**아케이드 계열** (하위 폴더)

`BasketBallMiniGame`, `BeadCatcherMiniGame`, `BrickMiniGame`, `BulletHellMiniGame`, `CarMiniGame`, `RayCast`

---

## 미니게임 추가 방법

1. `StageScreen`을 상속한 스크립트를 작성하고 `StartMiniGame()`을 구현합니다.
2. 완료 시 `MiniGameManager.NotifySuccess()` 또는 `MiniGameManager.NotifyFail()`을 호출합니다.
3. 프리팹을 만듭니다. `buttonHandler`라는 이름의 필드는 `MiniGameRuntimeBinder`가 자동으로 연결합니다.
4. Inspector에서 `MiniGamePool`의 해당 스테이지 `StageMiniGameGroup`에 프리팹을 추가합니다.
5. **스포너·매니저 코드는 수정할 필요가 없습니다.**

---

## 주의사항

- **`Time.timeScale = 0`**: 팝업이 열리면 게임 로직이 멈춥니다.
  팝업 중에도 동작해야 하는 타이머·애니메이션은 반드시 `Time.unscaledDeltaTime`을 사용하세요.
- **UI 텍스트는 전부 한국어**입니다. 약관·개인정보처리방침 등 장문 텍스트는
  `Assets/Resources/GameTexts.json`에서 런타임에 로드합니다.
- `MiniGameManager`는 `[RuntimeInitializeOnLoadMethod]`로 정적 이벤트를 초기화합니다.
  (에디터 Play 모드 재진입 시 이벤트 누수 방지)

---

## 디렉토리 구조

```
Assets/
├── Scenes/                    # 메인 씬(TongHapTest) + 미니게임 단독 씬
├── Scripts/
│   ├── MiniGame/              # 미니게임 구현체 (12개+)
│   ├── MiniGameSpawner/       # 스포너 partial 클래스 + 풀 / 바인더 / 게이트 / 라우터
│   ├── Popup/                 # 팝업 partial 클래스 + 설정 / 송금 팝업
│   └── *.cs                   # 매니저, 타이머, 오디오, UI
├── Prefabs/MiniGamePanels/    # 미니게임 UI 프리팹
└── Resources/GameTexts.json   # 런타임 로드 텍스트 데이터
```

---

## 코드 컨벤션

- 포매터: **CSharpier 1.2.6** (`.config/dotnet-tools.json`으로 관리)

  ```bash
  dotnet tool restore && dotnet csharpier format .
  ```

- 줄바꿈: `.gitattributes` + `.editorconfig`로 C# 파일 **LF** 강제
- 브랜치: `main`에서 분기, 이슈 번호를 포함한 이름 사용 (예: `refactor/54-minigame-spawner-cleanup`)
- 커밋: `type(scope): 요약` 형식 (예: `refactor(MiniGameSpawner): ScreenRouter 도입`)
- 이슈: `.github/ISSUE_TEMPLATE/`의 버그 리포트 / 기능 요청 템플릿 사용

---

## 팀

| 이름 | GitHub / 연락처 |
|---|---|
| 이현진 | zse846@gmail.com |
| 김준영 | suk558165@gmail.com |
| 박우빈 | oi3oi3oi@naver.com |

리포지토리: [Devel-Rocket-ClassRoom/toy-project-0-team-4](https://github.com/Devel-Rocket-ClassRoom/toy-project-0-team-4)
