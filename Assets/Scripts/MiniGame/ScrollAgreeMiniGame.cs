using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollAgreeMiniGame : MonoBehaviour
{
    private ScrollRect scrollRect;
    private Button agreeButton;
    private Button disagreeButton;
    private CanvasGroup agreeGroup;
    private TextMeshProUGUI hintText;
    private bool agreeUnlocked = false;

    private const string TermsContent =
        "<b>개인정보 처리방침 및 서비스 이용약관</b>\n\n" +
        "본 약관은 최종 수정일: 2025년 1월 1일부터 시행됩니다.\n\n" +

        "제1조 (목적)\n본 약관은 주식회사 ToyCorp(이하 '회사')이 제공하는 모든 온라인 및 모바일 서비스의 " +
        "이용과 관련하여 회사와 이용자 간의 권리, 의무 및 책임사항, 서비스 이용조건 및 절차, " +
        "기타 필요한 사항을 규정함을 목적으로 합니다.\n\n" +

        "제2조 (정의)\n본 약관에서 사용하는 용어의 정의는 다음과 같습니다.\n" +
        "① '서비스'란 회사가 제공하는 모든 온라인 게임, 커뮤니티, 콘텐츠 플랫폼을 의미합니다.\n" +
        "② '이용자'란 본 약관에 동의하고 회사가 제공하는 서비스를 이용하는 자를 말합니다.\n" +
        "③ '계정'이란 이용자가 서비스를 이용하기 위해 설정한 아이디 및 비밀번호의 조합을 말합니다.\n" +
        "④ '콘텐츠'란 서비스 내에서 이용자가 이용할 수 있는 모든 디지털 저작물을 의미합니다.\n\n" +

        "제3조 (약관의 게시 및 개정)\n" +
        "① 회사는 본 약관의 내용을 이용자가 쉽게 알 수 있도록 서비스 초기화면에 게시합니다.\n" +
        "② 회사는 필요한 경우 관련 법령을 위배하지 않는 범위에서 본 약관을 개정할 수 있습니다.\n" +
        "③ 회사가 약관을 개정할 경우에는 적용일자 및 개정사유를 명시하여 서비스 내에 " +
        "그 적용일자 7일 이전부터 공지합니다.\n\n" +

        "제4조 (수집하는 개인정보)\n회사는 서비스 제공을 위하여 아래와 같은 개인정보를 수집합니다.\n" +
        "▶ 필수 수집 항목\n" +
        "- 이름, 이메일 주소, 전화번호, 생년월일, 성별\n" +
        "- 서비스 이용 기록, 접속 로그, 쿠키, IP 주소, 불량 이용 기록\n" +
        "- 결제 정보 (신용카드 번호, 은행 계좌 정보, 결제 이력)\n" +
        "- 기기 고유 식별자, OS 정보, 브라우저 종류 및 버전\n" +
        "▶ 선택 수집 항목\n" +
        "- 프로필 사진, 닉네임, 자기소개\n" +
        "- 위치 정보 (이용자 동의 시에만 수집)\n" +
        "- SNS 연동 정보 (연동 시에만 수집)\n\n" +

        "제5조 (개인정보의 이용 목적)\n수집한 개인정보는 다음의 목적으로 이용됩니다.\n" +
        "① 서비스 제공, 콘텐츠 제공, 맞춤 서비스 제공, 본인 인증, 연령 확인\n" +
        "② 회원 관리: 회원제 서비스 이용, 개인식별, 가입 의사 확인, 불량회원 부정 이용 방지\n" +
        "③ 마케팅 및 광고 활용: 신규 서비스 안내, 이벤트 정보 제공, 맞춤형 광고 제공\n" +
        "④ 서비스 개선: 접속 빈도 파악, 이용자의 서비스 이용에 대한 통계 분석\n" +
        "⑤ 분쟁 조정 및 민원 처리: 고충 처리, 법적 분쟁 해결\n\n" +

        "제6조 (개인정보의 보유 및 이용 기간)\n회사는 법령에 따른 개인정보 보유·이용기간 또는 정보주체로부터 " +
        "개인정보를 수집 시에 동의 받은 개인정보 보유·이용기간 내에서 개인정보를 처리·보유합니다.\n" +
        "- 계약 또는 청약철회 등에 관한 기록: 5년 (전자상거래법)\n" +
        "- 소비자의 불만 또는 분쟁처리에 관한 기록: 3년 (전자상거래법)\n" +
        "- 대금결제 및 재화 등의 공급에 관한 기록: 5년 (전자상거래법)\n" +
        "- 웹사이트 방문 기록: 3개월 (통신비밀보호법)\n" +
        "- 표시·광고에 관한 기록: 6개월 (전자상거래법)\n\n" +

        "제7조 (개인정보의 제3자 제공)\n회사는 원칙적으로 이용자의 개인정보를 외부에 제공하지 않습니다. " +
        "다만, 아래의 경우에는 예외로 합니다.\n" +
        "① 이용자가 사전에 동의한 경우\n" +
        "② 법령의 규정에 의거하거나, 수사 목적으로 법령에 정해진 절차와 방법에 따라 수사기관의 요구가 있는 경우\n" +
        "③ 서비스 제공을 위해 외부 전문업체에 위탁하는 경우 (수탁업체 목록은 홈페이지 공지)\n\n" +

        "제8조 (이용자의 권리 및 의무)\n이용자는 개인정보주체로서 아래와 같은 권리를 행사할 수 있습니다.\n" +
        "- 개인정보 열람 요구\n" +
        "- 오류 등이 있을 경우 정정 요구\n" +
        "- 삭제 요구\n" +
        "- 처리 정지 요구\n" +
        "- 개인정보 이동 요구 (법령에서 허용하는 범위 내)\n\n" +
        "이용자는 다음 각 호의 행위를 하여서는 안 됩니다.\n" +
        "① 타인의 개인정보 또는 계정 정보를 도용하는 행위\n" +
        "② 서비스를 이용하여 법령 또는 이 약관이 금지하거나 공서양속에 반하는 행위\n" +
        "③ 회사의 서비스 운영을 방해하는 행위\n" +
        "④ 자동화된 수단(봇, 매크로 등)을 이용하여 서비스를 이용하는 행위\n" +
        "⑤ 회사의 사전 동의 없이 서비스를 이용하여 영리 목적의 활동을 하는 행위\n\n" +

        "제9조 (개인정보 보호책임자)\n회사는 개인정보 처리에 관한 업무를 총괄해서 책임지고, 이용자의 개인정보 관련 " +
        "불만처리 및 피해구제 등을 위하여 아래와 같이 개인정보 보호책임자를 지정하고 있습니다.\n" +
        "▶ 개인정보 보호책임자\n" +
        "성명: 홍길동  |  직책: 개인정보 보호팀장\n" +
        "연락처: privacy@toycorp.co.kr  |  전화: 02-1234-5678\n" +
        "▶ 개인정보 보호 담당 부서\n" +
        "부서명: 정보보안팀  |  담당자: 김보안\n" +
        "연락처: security@toycorp.co.kr\n\n" +

        "제10조 (쿠키의 사용)\n" +
        "① 회사는 이용자에게 개별적인 맞춤서비스를 제공하기 위해 이용정보를 저장하고 " +
        "수시로 불러오는 '쿠키(cookie)'를 사용합니다. 쿠키는 웹사이트를 운영하는데 이용되는 서버가 " +
        "이용자의 컴퓨터 브라우저에 보내는 소량의 정보이며 이용자들의 PC 컴퓨터 내의 하드디스크에 저장됩니다.\n" +
        "② 이용자는 쿠키 설치에 대한 선택권을 가지고 있습니다. 웹브라우저에서 옵션을 설정함으로써 " +
        "모든 쿠키를 허용하거나, 쿠키가 저장될 때마다 확인을 거치거나, 아니면 모든 쿠키의 저장을 거부할 수도 있습니다. " +
        "단, 쿠키의 저장을 거부할 경우에는 로그인이 필요한 일부 서비스는 이용에 어려움이 있을 수 있습니다.\n\n" +

        "제11조 (개인정보의 안전성 확보 조치)\n회사는 개인정보보호법 제29조에 따라 다음과 같이 안전성 확보에 " +
        "필요한 기술적/관리적 및 물리적 조치를 하고 있습니다.\n" +
        "1. 개인정보 취급 직원의 최소화 및 교육 실시 (연 2회 이상 의무 교육)\n" +
        "2. 정기적인 자체 감사 실시 (분기별 1회 이상)\n" +
        "3. 개인정보에 대한 접근 제한 (권한 관리 시스템 운영, 다중 인증 적용)\n" +
        "4. 개인정보의 암호화 (전송 구간 TLS 1.3, 저장 데이터 AES-256 이상 적용)\n" +
        "5. 해킹 등에 대비한 기술적 대책 (방화벽, IDS/IPS, WAF 운영)\n" +
        "6. 개인정보 처리시스템 접근 기록 보관 (최소 6개월 이상)\n" +
        "7. 보안프로그램 설치 및 주기적 갱신·점검 (자동 업데이트 적용)\n" +
        "8. 물리적 접근 제한 (서버실 출입 통제, CCTV 설치)\n\n" +

        "제12조 (서비스 이용 요금)\n" +
        "① 기본 서비스는 무료로 제공됩니다. 단, 일부 유료 콘텐츠 및 프리미엄 기능은 별도 요금이 부과됩니다.\n" +
        "② 유료 서비스 이용 요금은 서비스 내 결제 안내 페이지에서 확인할 수 있습니다.\n" +
        "③ 회사는 서비스 요금을 변경하는 경우 변경일 30일 전에 서비스 내 공지사항을 통해 안내합니다.\n" +
        "④ 이용자는 요금 변경에 동의하지 않을 경우 서비스 이용을 중단하고 탈퇴할 수 있습니다.\n\n" +

        "제13조 (서비스의 중단)\n" +
        "① 회사는 컴퓨터 등 정보통신설비의 보수점검, 교체 및 고장, 통신의 두절 등의 사유가 발생한 경우에는 " +
        "서비스의 제공을 일시적으로 중단할 수 있습니다.\n" +
        "② 회사는 제1항의 사유로 서비스의 제공이 일시적으로 중단됨으로 인하여 이용자 또는 제3자가 입은 손해에 대하여 배상합니다. " +
        "단, 회사의 고의 또는 과실이 없는 경우에는 그러하지 아니합니다.\n" +
        "③ 사업 종목의 전환, 사업의 포기, 업체 간의 통합 등의 이유로 서비스를 제공할 수 없게 되는 경우에는 " +
        "회사는 제8조에 정한 방법으로 이용자에게 통지하고 당초 회사에서 제시한 조건에 따라 소비자에게 보상합니다.\n\n" +

        "제14조 (분쟁 해결)\n" +
        "① 회사는 이용자가 제기하는 정당한 의견이나 불만을 반영하고 그 피해를 보상처리하기 위하여 " +
        "피해보상처리기구를 설치·운영합니다.\n" +
        "② 회사와 이용자 간에 발생한 분쟁은 전자거래기본법 제28조 및 동 시행령 제15조에 의하여 설치된 " +
        "전자거래분쟁조정위원회에 조정을 신청할 수 있습니다.\n" +
        "③ 본 약관 또는 서비스와 관련하여 발생하는 분쟁에 대해서는 대한민국 법원을 관할 법원으로 합니다.\n\n" +

        "제15조 (저작권 및 지식재산권)\n" +
        "① 회사가 작성한 저작물에 대한 저작권 기타 지식재산권은 회사에 귀속합니다.\n" +
        "② 이용자는 회사를 이용함으로써 얻은 정보 중 회사에게 지식재산권이 귀속된 정보를 " +
        "회사의 사전 승낙 없이 복제, 송신, 출판, 배포, 방송 기타 방법에 의하여 영리목적으로 이용하거나 " +
        "제3자에게 이용하게 하여서는 안 됩니다.\n\n" +

        "제16조 (기타)\n" +
        "① 회사가 각 개별서비스에 대한 별도 약관 및 이용조건을 정하여 이를 이용자에게 고지할 경우에는 " +
        "본 약관보다 우선 적용됩니다.\n" +
        "② 본 약관에 명시되지 않은 사항은 관련 법령 및 상관례에 따릅니다.\n" +
        "③ 본 약관은 한국어로 작성되었으며, 번역본과 한국어본이 상충하는 경우 한국어본이 우선합니다.\n\n" +

        "<b>위 약관의 내용을 모두 확인하셨다면 아래의 [동의] 버튼이 활성화됩니다.</b>";

    public void StartMiniGame()
    {
        BuildUI();
    }

    void BuildUI()
    {
        var bg = CreatePanel("Background", transform, Color.white);
        var bgRect = bg.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = bgRect.offsetMax = Vector2.zero;

        var titleGO = new GameObject("Title", typeof(RectTransform));
        titleGO.transform.SetParent(bg.transform, false);
        var titleRect = titleGO.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0f, 1f);
        titleRect.anchorMax = new Vector2(1f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = Vector2.zero;
        titleRect.sizeDelta = new Vector2(0f, 60f);

        var titleTmp = titleGO.AddComponent<TextMeshProUGUI>();
        titleTmp.text = "서비스 이용약관";
        titleTmp.fontSize = 24;
        titleTmp.fontStyle = FontStyles.Bold;
        titleTmp.alignment = TextAlignmentOptions.Center;
        titleTmp.color = new Color(0.1f, 0.1f, 0.1f);

        var viewportGO = CreatePanel("Viewport", bg.transform, new Color(0.95f, 0.95f, 0.95f));
        var viewportRect = viewportGO.GetComponent<RectTransform>();
        viewportRect.anchorMin = new Vector2(0f, 0.15f);
        viewportRect.anchorMax = new Vector2(1f, 1f);
        viewportRect.offsetMin = new Vector2(10f, 0f);
        viewportRect.offsetMax = new Vector2(-10f, -65f);
        viewportGO.AddComponent<RectMask2D>();

        var contentGO = new GameObject("Content", typeof(RectTransform));
        contentGO.transform.SetParent(viewportGO.transform, false);
        var contentRect = contentGO.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0f, 1f);
        contentRect.anchorMax = new Vector2(1f, 1f);
        contentRect.pivot = new Vector2(0.5f, 1f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = Vector2.zero;

        var csf = contentGO.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var contentText = contentGO.AddComponent<TextMeshProUGUI>();
        contentText.text = TermsContent;
        contentText.fontSize = 14;
        contentText.color = new Color(0.15f, 0.15f, 0.15f);
        contentText.margin = new Vector4(12f, 10f, 12f, 10f);

        var scrollGO = new GameObject("ScrollRect", typeof(RectTransform));
        scrollGO.transform.SetParent(viewportGO.transform, false);
        var scrollRectRT = scrollGO.GetComponent<RectTransform>();
        scrollRectRT.anchorMin = Vector2.zero;
        scrollRectRT.anchorMax = Vector2.one;
        scrollRectRT.offsetMin = scrollRectRT.offsetMax = Vector2.zero;

        scrollRect = scrollGO.AddComponent<ScrollRect>();
        scrollRect.content = contentRect;
        scrollRect.viewport = viewportRect;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.scrollSensitivity = 30f;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;

        var scrollbarGO = new GameObject("Scrollbar", typeof(RectTransform));
        scrollbarGO.transform.SetParent(bg.transform, false);
        var scrollbarRect = scrollbarGO.GetComponent<RectTransform>();
        scrollbarRect.anchorMin = new Vector2(1f, 0.15f);
        scrollbarRect.anchorMax = new Vector2(1f, 1f);
        scrollbarRect.pivot = new Vector2(1f, 0.5f);
        scrollbarRect.anchoredPosition = new Vector2(0f, -32.5f);
        scrollbarRect.sizeDelta = new Vector2(12f, -65f);

        var scrollbarBg = scrollbarGO.AddComponent<Image>();
        scrollbarBg.color = new Color(0.8f, 0.8f, 0.8f);

        var scrollbar = scrollbarGO.AddComponent<Scrollbar>();
        scrollbar.direction = Scrollbar.Direction.BottomToTop;

        var handleGO = new GameObject("Handle", typeof(RectTransform));
        handleGO.transform.SetParent(scrollbarGO.transform, false);
        var handleRect = handleGO.GetComponent<RectTransform>();
        handleRect.anchorMin = Vector2.zero;
        handleRect.anchorMax = Vector2.one;
        handleRect.sizeDelta = Vector2.zero;
        var handleImg = handleGO.AddComponent<Image>();
        handleImg.color = new Color(0.4f, 0.4f, 0.4f);
        scrollbar.handleRect = handleRect;
        scrollbar.targetGraphic = handleImg;

        scrollRect.verticalScrollbar = scrollbar;
        scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;

        var hintGO = new GameObject("Hint", typeof(RectTransform));
        hintGO.transform.SetParent(bg.transform, false);
        var hintRect = hintGO.GetComponent<RectTransform>();
        hintRect.anchorMin = new Vector2(0f, 0.09f);
        hintRect.anchorMax = new Vector2(1f, 0.15f);
        hintRect.offsetMin = hintRect.offsetMax = Vector2.zero;
        hintText = hintGO.AddComponent<TextMeshProUGUI>();
        hintText.text = "▼ 끝까지 스크롤 하세요";
        hintText.fontSize = 13;
        hintText.alignment = TextAlignmentOptions.Center;
        hintText.color = new Color(0.6f, 0.2f, 0.2f);

        var btnArea = new GameObject("ButtonArea", typeof(RectTransform));
        btnArea.transform.SetParent(bg.transform, false);
        var btnAreaRect = btnArea.GetComponent<RectTransform>();
        btnAreaRect.anchorMin = new Vector2(0f, 0f);
        btnAreaRect.anchorMax = new Vector2(1f, 0.09f);
        btnAreaRect.offsetMin = new Vector2(10f, 5f);
        btnAreaRect.offsetMax = new Vector2(-10f, -5f);

        disagreeButton = CreateButton("거절", btnArea.transform,
            new Vector2(0f, 0f), new Vector2(0.45f, 1f),
            new Color(0.85f, 0.2f, 0.2f));
        disagreeButton.onClick.AddListener(() => MiniGameManager.NotifyFail());

        var agreeBtnGO = CreateButtonGO("동의", btnArea.transform,
            new Vector2(0.55f, 0f), new Vector2(1f, 1f),
            new Color(0.3f, 0.6f, 0.3f));
        agreeButton = agreeBtnGO.GetComponent<Button>();
        agreeGroup = agreeBtnGO.AddComponent<CanvasGroup>();
        agreeGroup.alpha = 0.35f;
        agreeGroup.interactable = false;
        agreeGroup.blocksRaycasts = false;
        agreeButton.onClick.AddListener(() => MiniGameManager.NotifySuccess());
    }

    void Update()
    {
        if (agreeUnlocked || scrollRect == null) return;

        if (scrollRect.verticalNormalizedPosition <= 0.02f)
        {
            agreeUnlocked = true;
            agreeGroup.alpha = 1f;
            agreeGroup.interactable = true;
            agreeGroup.blocksRaycasts = true;
            if (hintText != null)
            {
                hintText.text = "✓ 동의 버튼이 활성화되었습니다";
                hintText.color = new Color(0.1f, 0.5f, 0.1f);
            }
        }
    }

    GameObject CreatePanel(string name, Transform parent, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = color;
        return go;
    }

    Button CreateButton(string label, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Color color)
    {
        return CreateButtonGO(label, parent, anchorMin, anchorMax, color).GetComponent<Button>();
    }

    GameObject CreateButtonGO(string label, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Color color)
    {
        var go = new GameObject(label, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        var img = go.AddComponent<Image>();
        img.color = color;
        go.AddComponent<Button>();

        var textGO = new GameObject("Text", typeof(RectTransform));
        textGO.transform.SetParent(go.transform, false);
        var textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = textRect.offsetMax = Vector2.zero;

        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 18;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        return go;
    }
}
