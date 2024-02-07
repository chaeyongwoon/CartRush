# FrameWork

FrameWork

### 시작 할때 Checklist

- Product Name 바꿔주기

### 맨 처음 쓴다면! 

- Use URP
- 
- Scene 마다 해당 BaseScene을 상속받는 Scene오브젝트를 넣어줄것!
- 
- Managers 를 호출하면 Managers 저절로 생성됨
- Managers 에서 타겟프레임 60으로 잡았고 멀티 터치 안되게 막아놓았음!
- Managers 는 DontDestroy 이고 나머지 매니저는 Managers에서 생산함
- 새로운씬으로 갈때마다 Managers에서 Clear가 호출됨
- 
- DataManger는 Managers.Data 로 호출되며 EasySave3을 사용함
- EasySave에 대해 정리 되있는 링크 https://mentum.tistory.com/156 
- 
- 풀링하는 객체는 Poolable 스크립트를 달아주어야 함
- 풀링매니저는 왠만하면 안써도 됨, 리소스 매니저의 Instance와 Destroy를 활용할 것
- 
- 리소스 매니저를 활용하기 위해 Addressables을 사용해야함!
- 리소스 매니저에서 인스턴스 된것은 async로 반환되므로 Instnace된 오브젝트를 바로 사용시에 OnComplete 콜백에서 처리 하거나 await로 기다려서 처리
- 모든 리소스는 리소스 매니저를 활용해서 처리할 것!
- Sprite는 Sprite Atlas로 묶어 아틀라스만 어드레서블로 등록하여 사용, 이때 Label을 Sprite Atlas로 지정해 줄것
- 빌드 전에 Addressable Groups 에서 Build > New Build > Default Build Script 를 클릭하여 에셋 먼저 빌드하고 메인 빌드 진행 할것
- 
- 씬 이동은 Scene 매니저를 활용함
- 씬 이동시에 SceneTrasition 애니메이션 실행 됨
- Define.Scene 에 씬들의 이름을 넣어주어야함
- 
- 사운드 관련내용은 Managers.Sound
- UI 관련 내용은 Managers.UI
- 해보고 모르겠으면 문의!!
- 
- Utils 폴더에는 여러 스크립트가 있는데
- Define 은 상수정의나 Enum정의때 사용
- Extension은 Extension기능을 위한 기능 추가 모음집
- Util 은 아무곳에나 활용가능한 Static 함수 집합
- 
- 
- 
- 씬에서 우클릭시 겹쳐있는 오브젝트 리스트가 나옴 (Selection Utility)
- 
- 프로젝트 탭에서 폴더 Alt + 클릭 하면 폴더 색 바꿀수 있음 (Rainbow Folder2)
- 
- Odin 관련 내용은 https://mentum.tistory.com/388 여기나 Tools > Odin Inspector > Attribute Example 을 활용하여 사용
- 
- Maintainer는 사용하는 레퍼런스를 알려주고 정리하는 에셋
- 
- Obfuscator는 난독화 에셋 // ObfuscatorOptionsImport 세팅으로 옵션 변경가능

- Anti-Cheat Toolkit 은 메모리 탐지에 조금 더 안전하도록 하는 에셋  https://mentum.tistory.com/390- 




### Imported Assets

- 2D Sprite        Version 1.0.0 - December 08, 2021
- Cinemachine      Version 2.6.11 - November 03, 2021
- UniversalPR      Version 10.7.0 - September 21, 2021
- Addressables     Version 1.18.19 - October 22, 2021
- 
- DOTween          Version 1.0.310 - August 04, 2021 
- Easy Save        Version 3.4.0 - January 04, 2022
- Rainbow Folder2  Version 2.3.0 - February 26, 2021
- Selection Utility Version 1.2 - April 20, 2021
- Odin             Version 3.0.12 - November 15, 2021
- Maintainer       Version 1.14.1 - April 12, 2022
- Anti-Cheat Toolkit 2021 Version 2021.1.1 - May 05, 2022
- Obfuscator        Version 3.9.4 - April 08, 2022