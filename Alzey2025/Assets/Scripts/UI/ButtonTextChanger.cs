using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class ButtonTextChanger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

        [SerializeField]
        TMP_Text text;

        [SerializeField]
        Color hoverColor = Color.white;

        [SerializeField]
        Color clickColor = Color.white;

        Color initialColor;
        bool hovering;

        private void Start() {
            initialColor = text.color;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            text.color = hoverColor;
            hovering = true;
        }

        public void OnPointerDown(PointerEventData eventData) {
            text.color = clickColor;
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (hovering) {
                text.color = hoverColor;

                return;
            }

            text.color = initialColor;
        }

        public void OnPointerExit(PointerEventData eventData) {
            text.color = initialColor;
            hovering = false;
        }
    }
}
