using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollAgreeMiniGame : MonoBehaviour
{
    [Header("UI 참조 (Inspector에서 연결)")]
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;
    [SerializeField] private ScrollRect scrollRect;

    [Header("비활성 상태 투명도 (0=완전투명, 1=불투명)")]
    [SerializeField] [Range(0f, 1f)] private float disabledAlpha = 0.3f;

    [Header("스크롤 감도")]
    [SerializeField] private float scrollSensitivity = 3f;

    [Header("활성화 오프셋 (양수=더 내려야 활성화)")]
    [SerializeField] private float activateOffset = 30f;

    private readonly string scrollText =
@"개인정보 처리방침 및 서비스 이용약관

본 약관은 최종 수정일: 2025년 1월 1일부터 시행됩니다.

제1조 (목적)
본 약관은 주식회사 ToyCorp(이하 '회사')이 제공하는 모든 온라인 및 모바일 서비스의 이용과 관련하여 회사와 이용자 간의 권리, 의무 및 책임사항, 서비스 이용조건 및 절차, 기타 필요한 사항을 규정함을 목적으로 합니다.

제2조 (정의)
본 약관에서 사용하는 용어의 정의는 다음과 같습니다.
① '서비스'란 회사가 제공하는 모든 온라인 게임, 커뮤니티, 콘텐츠 플랫폼을 의미합니다.
② '이용자'란 본 약관에 동의하고 회사가 제공하는 서비스를 이용하는 자를 말합니다.
③ '계정'이란 이용자가 서비스를 이용하기 위해 설정한 아이디 및 비밀번호의 조합을 말합니다.
④ '콘텐츠'란 서비스 내에서 이용자가 이용할 수 있는 모든 디지털 저작물을 의미합니다.

제3조 (약관의 게시 및 개정)
① 회사는 본 약관의 내용을 이용자가 쉽게 알 수 있도록 서비스 초기화면에 게시합니다.
② 회사는 필요한 경우 관련 법령을 위배하지 않는 범위에서 본 약관을 개정할 수 있습니다.
③ 회사가 약관을 개정할 경우에는 적용일자 및 개정사유를 명시하여 서비스 내에 그 적용일자 7일 이전부터 공지합니다.

제4조 (수집하는 개인정보)
회사는 서비스 제공을 위하여 아래와 같은 개인정보를 수집합니다.
▶ 필수 수집 항목
- 이름, 이메일 주소, 전화번호, 생년월일, 성별
- 서비스 이용 기록, 접속 로그, 쿠키, IP 주소, 불량 이용 기록
- 결제 정보 (신용카드 번호, 은행 계좌 정보, 결제 이력)
- 기기 고유 식별자, OS 정보, 브라우저 종류 및 버전
▶ 선택 수집 항목
- 프로필 사진, 닉네임, 자기소개
- 위치 정보 (이용자 동의 시에만 수집)
- SNS 연동 정보 (연동 시에만 수집)

제5조 (개인정보의 이용 목적)
수집한 개인정보는 다음의 목적으로 이용됩니다.
① 서비스 제공, 콘텐츠 제공, 맞춤 서비스 제공, 본인 인증, 연령 확인
② 회원 관리: 회원제 서비스 이용, 개인식별, 가입 의사 확인, 불량회원 부정 이용 방지
③ 마케팅 및 광고 활용: 신규 서비스 안내, 이벤트 정보 제공, 맞춤형 광고 제공
④ 서비스 개선: 접속 빈도 파악, 이용자의 서비스 이용에 대한 통계 분석
⑤ 분쟁 조정 및 민원 처리: 고충 처리, 법적 분쟁 해결

제6조 (개인정보의 보유 및 이용 기간)
회사는 법령에 따른 개인정보 보유·이용기간 내에서 개인정보를 처리·보유합니다.
- 계약 또는 청약철회 등에 관한 기록: 5년 (전자상거래법)
- 소비자의 불만 또는 분쟁처리에 관한 기록: 3년 (전자상거래법)
- 대금결제 및 재화 등의 공급에 관한 기록: 5년 (전자상거래법)
- 웹사이트 방문 기록: 3개월 (통신비밀보호법)
- 표시·광고에 관한 기록: 6개월 (전자상거래법)

제7조 (개인정보의 제3자 제공)
회사는 원칙적으로 이용자의 개인정보를 외부에 제공하지 않습니다. 다만, 아래의 경우에는 예외로 합니다.
① 이용자가 사전에 동의한 경우
② 법령의 규정에 의거하거나, 수사 목적으로 법령에 정해진 절차와 방법에 따라 수사기관의 요구가 있는 경우
③ 서비스 제공을 위해 외부 전문업체에 위탁하는 경우 (수탁업체 목록은 홈페이지 공지)

제8조 (이용자의 권리 및 의무)
이용자는 개인정보주체로서 아래와 같은 권리를 행사할 수 있습니다.
- 개인정보 열람 요구
- 오류 등이 있을 경우 정정 요구
- 삭제 요구
- 처리 정지 요구
- 개인정보 이동 요구 (법령에서 허용하는 범위 내)
이용자는 다음 각 호의 행위를 하여서는 안 됩니다.
① 타인의 개인정보 또는 계정 정보를 도용하는 행위
② 서비스를 이용하여 법령 또는 이 약관이 금지하거나 공서양속에 반하는 행위
③ 회사의 서비스 운영을 방해하는 행위
④ 자동화된 수단(봇, 매크로 등)을 이용하여 서비스를 이용하는 행위
⑤ 회사의 사전 동의 없이 서비스를 이용하여 영리 목적의 활동을 하는 행위";

    private float textHeight;

    public void StartMiniGame()
    {
        if (scrollRect == null)
            scrollRect = GetComponentInChildren<ScrollRect>(true);

        scrollRect.scrollSensitivity = scrollSensitivity;

        SetAgreeButtonState(false);

        disagreeButton.onClick.AddListener(() => MiniGameManager.NotifyFail());
        agreeButton.onClick.AddListener(() => MiniGameManager.NotifySuccess());

        DisableDrag();
        StartCoroutine(FitContent());
    }

    private void SetAgreeButtonState(bool active)
    {
        agreeButton.interactable = active;

        if (!agreeButton.TryGetComponent<CanvasGroup>(out var canvasGroup))
            canvasGroup = agreeButton.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = active ? 1f : disabledAlpha;
    }

    private IEnumerator FitContent()
    {
        yield return new WaitForEndOfFrame();

        if (scrollRect == null || scrollRect.content == null) yield break;

        TextMeshProUGUI tmp = scrollRect.content.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            if (!string.IsNullOrEmpty(scrollText))
                tmp.text = scrollText;

            tmp.ForceMeshUpdate();

            // 마지막 줄 하단 Y 위치로 실제 텍스트 높이 계산
            var info = tmp.textInfo;
            if (info != null && info.lineCount > 0)
            {
                var lastLine = info.lineInfo[info.lineCount - 1];
                // descender는 음수(위에서 아래 방향), 절댓값이 실제 높이
                textHeight = Mathf.Abs(lastLine.descender);
            }
            else
            {
                textHeight = tmp.preferredHeight;
            }

            tmp.rectTransform.sizeDelta = new Vector2(tmp.rectTransform.sizeDelta.x, textHeight);
            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, textHeight);
        }

        yield return null;
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void DisableDrag()
    {
        if (scrollRect == null) return;

        if (scrollRect.verticalScrollbar != null)
            scrollRect.verticalScrollbar.interactable = false;

        scrollRect.horizontal = false;

        RectTransform viewport = scrollRect.viewport != null
            ? scrollRect.viewport
            : scrollRect.GetComponent<RectTransform>();

        GameObject blocker = new("DragBlocker", typeof(RectTransform));
        blocker.transform.SetParent(viewport, false);

        RectTransform rt = blocker.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var img = blocker.AddComponent<Image>();
        img.color = Color.clear;
        img.raycastTarget = true;

        blocker.AddComponent<ScrollDragBlocker>();
    }

    private void Update()
    {
        if (scrollRect == null || agreeButton.interactable || textHeight <= 0f) return;

        float viewportH = scrollRect.viewport.rect.height;
        float scrolled = scrollRect.content.anchoredPosition.y;

        // 텍스트 끝이 뷰포트 하단에 완전히 들어오면 활성화
        if (scrolled >= textHeight - viewportH + activateOffset)
            SetAgreeButtonState(true);
    }
}
