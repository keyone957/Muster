# MUSTER 아이돌 버튜버 콘서트 플랫폼

진행 기간: 2023.09 ~ 2024.02

사용한 기술 스택: C#, Meta Quest 2, Meta Quest Pro, Photon Fusion, Unity, VR

개발 인원(역할): 개발자 3명 + 디자이너 1명+ 기획자 3명

한 줄 설명: VR 버추얼 콘서트 플랫폼

비고: 팀 프로젝트/뉴콘텐츠 아카데미 단기과정 1기 MandU팀

## 프로젝트 소개

---


- 프로젝트 소개
    - 누구나 버츄얼 아이돌이 되어 쉽게 콘서트를 열 수 있는 플랫폼입니다.

[플랫폼 소개영상](https://www.youtube.com/watch?v=ZneKTgdCRMM)

<img src="https://github.com/user-attachments/assets/d42e12d5-4242-4a09-9a6c-39f307b10ca9" width="480" height="477">

---

## 나의 역할

---

1. VR 기기 연동.
    - IOBT 기술을 이용한 풀바디 트래킹 기능 구현.
    - VR기기 input값 연동 및 구성
    - 포톤 서버를 이용한 아이돌 페이셜 및 풀바디 트래킹 연동
    - 아바타 페이셜 트래킹 기능
    - 관객 이모티콘 기능 구현
    - Oculus Meta Quest pro, Oculus Meta Quest 2 두 가지 기기 연동.
2. 서버 연결
    - 포톤 서버를 이용한 아이돌, 관객 연동.

![Image](https://github.com/user-attachments/assets/27d52bdd-0ff2-4af0-9ca7-22b193008193)

![Image](https://github.com/user-attachments/assets/ce72a52d-8cb8-44b3-a9f3-2a77ffdc057c)

### [트러블 슈팅]

- 프로젝트에 사용할 아바타에 IOBT기술을 적용시키면 아바타의 뼈대가 뭉게지는 문제
    
    **원인** : 유니티의 Transform Dof 와 Apply Root Motion 옵션이 IOBT의 실시간 추적 데이터와 충돌하여 뼈대가 완전히 뭉게져 트래킹이 이상하게 됨.
    
    **해결**: https://keyone957.tistory.com/10
    
- 콘서트장 플랫폼을 플레이하던 도중 특정 구간에서 급격하게 FPS가 하락하여 렉이 심해진 문제
    
    **원인** : VR환경에서는  72~ 90FPS를 유지해야 사용자에게 쾌적한 플레이 환경을 제공할 수 있음. 그러나 프로파일러를 사용해 분석해 본 결과 특정 구간에 배치 되어있는 오브젝트의 Vertices 수가 100만 개가 넘어가 프레임 수가 내려가 렉이 걸리는 것이었음.
    
    **해결** : 디자이너와 이 문제를 공유하여 문제의 오브젝트를 로우폴리 방식으로 변경해 달라고 요청. 특정 구간에서 65-68FPS가 되어 렉이 먹었지만, 문제해결 이후 안정적으로 72-78FPS를 유지하면서 쾌적하게 플레이가 가능했었음.
    

## 개발 내용 및 플레이 영상

---

### [콘서트 시작]

![Image](https://github.com/user-attachments/assets/b3eb7421-3b7c-4498-a558-cea061268a3a)

⇒ 유저는 시작이 아이돌, 관객으로 구분이 되어 역할을 정할 수 있습니다.

<p align="left">
    <img src="https://github.com/user-attachments/assets/76318f07-c5e7-4ce3-aaec-19272f44edb0" width="480" height="300"/>
    <img src="https://github.com/user-attachments/assets/ed7bd5fc-ccc5-4b83-aad9-83853d480cd4" width="480" height="300"/>
</p>

⇒ 아이돌 역할을 선택할 때 콘서트 중 필요한 조명, 특수효과 및 무대 빛 효과를 사전에 설정이 가능합니다.

또한 시간을 설정하여 어느 시간에 연출 및 효과를 나오게 할지도 설정이 가능합니다

<p align="left">
    <img src="https://github.com/user-attachments/assets/382652d0-374d-4b63-a488-6181c520ae39" width="480" height="300"/>
    <img src="https://github.com/user-attachments/assets/4111839b-35c5-4dad-9d23-e67290de9683" width="480" height="300"/>
</p>

⇒ 유저는 콘서트장에 입장 시 아이돌이 무대를 개최하기 전에 자리를 배정받고 VR 기기 연결을 기다립니다.

### [게이미피케이션]

[게이미피케이션 영상](https://youtu.be/kwM9guyJpO4)

⇒만일 아이돌이 공연 도중 호응을 이끌어 내기 위해 게임을 시작하면 영상과 같이 응원봉을 흔들어 점수를 집계하여 유저참여형 콘텐츠를 제공합니다.

### [ 관객 이모티콘 사용 ]

<table>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/974632ff-7226-4913-829b-9986bcc409aa" width="480" height="300" alt="Player Attack"/><br/>
      이모티콘 사용하는 관객
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/ca1f2773-8409-401c-8dc9-8f671342cc90" width="480" height="300" alt="Skill Pattern"/><br/>
      이모티콘을 볼 수 있는 아이돌
    </td>
  </tr>
</table>


### [ 아이돌 ]

<table>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/e5248f9a-94ef-4b4b-a07e-6f72e1c59f5b" width="480" height="300" alt="Player Attack"/><br/>
      공연 연출 (불기둥,  폭죽)
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/09075951-4800-44bf-8a72-56370824de73" width="480" height="300" alt="Skill Pattern"/><br/>
      페이셜 및 상체 트래킹
    </td>
  </tr>
</table>

<p align="left">
    <img src="https://github.com/user-attachments/assets/c1c1a27b-f686-45b5-87b1-61ce47858e30" width="480" height="300"/>
    <img src="https://github.com/user-attachments/assets/725bb0af-897a-4405-ae97-c51188ff9753" width="480" height="300"/>
</p>

=> IOBT기술을 이용한 아바타 풀바디 트래킹

## 풀영상

- 관객
    
    [관객 입장](https://youtu.be/v0lw5rVbRik)
    
- 아이돌
    
    [아이돌 입장](https://youtu.be/3vQWHhON1PM)
    

## 프로젝트 사용 기술

---

### ⚒️ 클라이언트

- Unity
- C#
- Oculus Meta Sdk
- XR Interaction ToolKit

### ⚒️ 서버

- Photon Fushion

### ⚒️ 대응기기

- Oculus Meta Quest Pro
- Oculus Meta Quest 2

### ⚒️ 버전 관리 및 협업

- Git
- Notion
- Slack

### ⚒️ 개발 환경

- Visual Studio

## 프로젝트 일정 관리 및 협업

---

![Image](https://github.com/user-attachments/assets/858419dd-de67-4b22-abf1-b421d02c1893)

⇒일정관리 및 업무 분담

<p align="left">
    <img src="https://github.com/user-attachments/assets/5fecfb7f-2474-4552-9929-24a7cecc264b" width="480" height="300"/>
    <img src="https://github.com/user-attachments/assets/31894126-10bc-4271-afa9-f8c0f03f040f" width="480" height="300"/>
</p>

⇒ Git커밋메세지 및 브랜치 이름 규칙정하여 협업

![Image](https://github.com/user-attachments/assets/b3b234e0-41e1-4191-b857-674f180497f8)

⇒프로젝트를 진행하면서 문제해결 방법 및 패치노트 작성하여 아카이빙

## 프로젝트 성과 및 성취

---

이 프로젝트를 진행하기 전 VR에 관련된 경험이 없어서 처음 해보는 분야의 프로젝트였습니다. 하지만 저는 이 프로젝트를 원활하게 진행하기 위하여 VR관련 교육을 수강하고, 뉴콘텐츠 아카데미의 멘토링 서비스를 적극적으로 사용하여 프로젝트를 진행 하였습니다. 

Oculus Sdk 및 XR Interation Toolkit을 사용하여 어떻게 VR을 유저 및 아이돌과 연동하는 방법을 프로젝트를 진행하면서 배워 VR의 동작 방식 및 응용 방법을 배웠습니다. 또한 포톤 퓨전을 사용하여 유저와 아이돌과의 서버 연동을 하여 포톤 서버를 이용한 VR연동 방식을 배웠습니다.

---

프로젝트를 진행도중 프로젝트의 TA담당 역할을 하는 사람이 개인사정으로 인해 하차하는 상황이 생겨 아바타 관련 리깅하는 사람이 필요했습니다. 그 당시 아바타는 Mocopi를 사용하여 아바타 리깅 및 바디 트레킹을 하는 중이었습니다. 하지만 하차한 팀원이 개인 사정으로 인하여 인수인계를 해주지 못하고 나가 그 역할을 할 사람 및 대책이 필요했습니다. 개발자와 기획자가 회의 끝에 마침 저희가 쓰는 Oculus Meta에서 IOBT라는 하체를 트래킹해주는 기술을 출시하여 IOBT기술을 사용해, 제가 하차한 인원의 역할을 담당하였습니다. 이 사건 이후 개발자와 기획자가 더욱 자주 만나서 문제를 해결하여 성공적으로 뉴콘텐츠 아카데미 단기과정 1기팀으로써의 부스 운영을 마쳤습니다.
