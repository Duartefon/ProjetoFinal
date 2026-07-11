"""
Análise descritiva do questionário de validação do Ego Fractum.

Processa as respostas recolhidas em Google Forms / Excel, separa os dois
blocos de perguntas (compreensão do jogo vs. GUESS-18 adaptado), calcula
estatísticas descritivas (mediana, moda, desvio padrão) por item e por
subescala, e gera gráficos de barras divergentes com a distribuição de
respostas.

Bibliotecas: Matplotlib (gráficos), openpyxl (leitura do ficheiro .xlsx).

Uso:
    python analyze_questionnaire.py caminho/para/questionario.xlsx
"""

import sys
import statistics
from pathlib import Path

import matplotlib.pyplot as plt
import openpyxl


# Escala do bloco de compreensão do jogo
COMPREHENSION_SCALE = {
    "Strongly Disagree": 1,
    "Disagree": 2,
    "Undecided": 3,
    "Agree": 4,
    "Strongly Agree": 5,
}
COMPREHENSION_LABELS = ["Discordo\nTotalmente", "Discordo", "Indeciso",
                         "Concordo", "Concordo\nTotalmente"]

# Escala do bloco GUESS-18 adaptado.
GUESS_SCALE = {
    "Disagree Completely": 1,
    "Strongly Disagree": 1,
    "Disagree": 2,
    "No Opinion": 3,
    "Agree": 4,
    "Strongly Agree": 5,
    "Agree Completely": 5,
}
GUESS_LABELS = ["Discordo\nTotalmente", "Discordo", "Sem\nOpinião",
                 "Concordo", "Concordo\nTotalmente"]

# items que precisam de ser invertidos antes da agregação,
# tal como especificado no GUESS-18 original para "I feel bored".
REVERSE_ITEMS = {
    "I feel bored while playing the game.",
    "I found it a waste of time",
}

# compreensão do jogo (específico do projeto)
COMPREHENSION_ITEMS = [
    "At the start of the game, right after pressing play in the main menu, I understood that I had to open the door to be able to start walking",
    "After the starting room, the lighting in the hall above the doors helped me find the path to the generator room",
    "After finishing the generator I understood I had to go back to the starting hall using the elevator",
    "In the second puzzle, I was able to understand the room's visual elements were key to learn to how to finish the puzzle",
    "In the second puzzle, I was able to understand I had to measure certain weights to get the keypad's code",
    "While playing, I figured the controls of the game without outside help",
    "In the third puzzle, I understood what I had to do to start it",
    "In the third puzzle, I understood what my goal was to complete the puzzle",
    "In the third puzzle, the enemy gave me a good challenge",
]

# bloco 2a -> subescalas GUESS-18.
GUESS18_SUBSCALES = {
    "Usability/Playability": [
        "I find the controls of the game to be straightforward",
        "I find the game's interface to be easy to navigate",
    ],
    "Play Engrossment": [
        "I feel detached from the outside world while playing the game.",
        "I do not care to check events that are happening in the real world during the game.",
    ],
    "Enjoyment": [
        "I think the game is fun.",
        "I feel bored while playing the game.",  # reverse
    ],
    "Creative Freedom": [
        "I feel the game allows me to be imaginative.",
        "I feel creative while playing the game.",
    ],
    "Audio Aesthetics": [
        "I enjoy the sound effects in the game.",
        "I feel the game's audio (e.g., sound effects, music) enhances my gaming experience.",
    ],
    "Visual Aesthetics": [
        "I enjoy the game's graphics.",
        "I think the game is visually appealing.",
    ],
}

# bloco 2b -> coisas adicionais específicos do projeto (fora do GUESS-18
# original), agrupados por tema para facilitar a leitura dos resultados.
CUSTOM_ITEMS = {
    "Atmosfera e Presença": [
        "I find the game to be atmospheric",
        "I had a sense that I had returned from a journey",
    ],
    "Conforto em VR": [
        "I get motion sickness while playing the game",
        "I felt nauseous after playing",
    ],
    "Narrativa": [
        "I find the narrative easy to explore and understand",
    ],
    "Usabilidade (itens adicionais)": [
        "I find it easy to navigate the map",
        "I think it's easy to understand what am I supposed to do / where to go",
    ],
    "Gratificação e Desafio": [
        "Solving the game's puzzles brings me joy",
        "I felt satisfied",
        "I found it a waste of time",  # reverse
        "I felt like I completed a challenge",
    ],
}


# leitura e preparação dos dados

def load_data(path):
    workbook = openpyxl.load_workbook(path, data_only=True)
    sheet = workbook.active
    all_rows = list(sheet.iter_rows(values_only=True))

    header_row = all_rows[0]
    headers = []
    for h in header_row:
        headers.append(_clean(h))

    records = []
    for row in all_rows[1:]:
        record = {}
        for i in range(len(headers)):
            record[headers[i]] = _clean(row[i])
        records.append(record)
    return records


def _clean(value):
    if isinstance(value, str):
        cleaned = value.replace("\xa0", " ")
        cleaned = " ".join(cleaned.split())
        return cleaned.strip()
    return value


def code_response(raw_value, scale, item_text):
    code = scale.get(raw_value)
    if code is None:
        return None
    if item_text in REVERSE_ITEMS:
        code = 6 - code  # inverte 1<->5, 2<->4, mantém 3
    return code


def most_common_value(numbers):
    counts = {}
    for number in numbers:
        if number in counts:
            counts[number] += 1
        else:
            counts[number] = 1

    best_value = None
    best_count = -1
    for value in counts:
        if counts[value] > best_count:
            best_count = counts[value]
            best_value = value
    return best_value


def standard_deviation(numbers):
    if len(numbers) < 2:
        return None
    return round(statistics.stdev(numbers), 2)


# estatistica descritiva

def item_stats(records, item_text, scale):
    codes = []
    for r in records:
        codes.append(code_response(r[item_text], scale, item_text))

    valid_codes = []
    for code in codes:
        if code is not None:
            valid_codes.append(code)

    n_missing = len(codes) - len(valid_codes)

    if len(valid_codes) == 0:
        return dict(median=None, mode=None, stdev=None, n=0,
                    n_missing=n_missing, codes=codes)

    return dict(
        median=statistics.median(valid_codes),
        mode=most_common_value(valid_codes),
        stdev=standard_deviation(valid_codes),
        n=len(valid_codes),
        n_missing=n_missing,
        codes=codes,
    )


def subscale_scores(records, items, scale):
    """Para cada participante, calcula a média dos itens da subescala
    (ignorando itens em falta para esse participante). Devolve a lista de
    scores por participante mais a mediana/moda/desvio padrão agregados da
    subescala."""
    per_participant = []
    for r in records:
        codes = []
        for item in items:
            code = code_response(r[item], scale, item)
            if code is not None:
                codes.append(code)
        if len(codes) > 0:
            per_participant.append(round(statistics.mean(codes), 2))

    if len(per_participant) == 0:
        return dict(scores=[], median=None, mode=None, stdev=None)

    return dict(
        scores=per_participant,
        median=round(statistics.median(per_participant), 2),
        mode=most_common_value(per_participant),
        stdev=standard_deviation(per_participant),
    )


# graficos

def diverging_bar_chart(records, items, scale, labels, title, out_path,
                         short_labels=None):
    n_items = len(items)
    fig, ax = plt.subplots(figsize=(9, 0.9 * n_items + 1.5))

    colors = ["#b2182b", "#ef8a62", "#d9d9d9", "#67a9cf", "#2166ac"]
    y_positions = list(range(n_items))

    if short_labels:
        display_labels = short_labels
    else:
        display_labels = items

    missing_counts = []

    for row_idx in range(n_items):
        item = items[row_idx]

        codes = []
        for r in records:
            codes.append(code_response(r[item], scale, item))

        valid_codes = []
        for code in codes:
            if code is not None:
                valid_codes.append(code)

        n_missing = len(codes) - len(valid_codes)
        missing_counts.append(n_missing)

        if len(valid_codes) > 0:
            total_valid = len(valid_codes)
        else:
            total_valid = 1

        counts = {1: 0, 2: 0, 3: 0, 4: 0, 5: 0}
        for code in valid_codes:
            counts[code] += 1

        props = []
        for i in range(1, 6):
            props.append(counts[i] / total_valid * 100)

        neg = props[0] + props[1] + props[2] / 2
        pos = props[3] + props[4] + props[2] / 2

        left = -neg
        for i in range(5):
            p = props[i]
            c = colors[i]
            if i < 2:
                width = p
                ax.barh(row_idx, width, left=left, color=c, edgecolor="white")
                left += width
            elif i == 2:
                half = p / 2
                ax.barh(row_idx, p, left=-half, color=c, edgecolor="white")
            else:
                if i == 3:
                    left = props[2] / 2
                width = p
                ax.barh(row_idx, width, left=left, color=c, edgecolor="white")
                left += width

    ax.axvline(0, color="black", linewidth=0.8)
    ax.set_yticks(y_positions)
    ax.set_yticklabels(display_labels, fontsize=9)
    ax.set_xlabel("% de respostas (sem dados em falta)")
    ax.set_title(title, fontsize=11, fontweight="bold")
    ax.set_xlim(-100, 100)
    ax.set_xticks(range(-100, 101, 25))

    tick_labels = []
    for t in range(-100, 101, 25):
        tick_labels.append(f"{abs(t)}%")
    ax.set_xticklabels(tick_labels)

    handles = []
    for c in colors:
        handles.append(plt.Rectangle((0, 0), 1, 1, color=c))
    ax.legend(handles, labels, loc="upper center",
              bbox_to_anchor=(0.5, -0.15 - 0.02 * n_items), ncol=5, fontsize=8,
              frameon=False)

    for row_idx in range(len(missing_counts)):
        n_missing = missing_counts[row_idx]
        if n_missing:
            ax.annotate(f"{n_missing} sem resposta", xy=(101, row_idx),
                        va="center", fontsize=7, color="gray")

    fig.tight_layout()
    fig.savefig(out_path, dpi=200, bbox_inches="tight")
    plt.close(fig)
    print(f"Gráfico guardado em: {out_path}")


# main

def main(xlsx_path, out_dir="figuras"):
    out_dir = Path(out_dir)
    out_dir.mkdir(exist_ok=True)

    records = load_data(xlsx_path)
    n = len(records)
    print(f"Participantes carregados: {n}\n")

    print("=" * 70)
    print("BLOCO 1 - Compreensão do jogo ")
    print("=" * 70)
    for item in COMPREHENSION_ITEMS:
        s = item_stats(records, item, COMPREHENSION_SCALE)
        print(f"- {item[:60]:<60} | mediana={s['median']} moda={s['mode']} "
              f"dp={s['stdev']} n={s['n']}/{n}")

    short_comp = [
        "Compreendeu que tinha de abrir a porta",
        "Iluminação ajudou a encontrar o gerador",
        "Compreendeu que tinha de voltar pelo elevador",
        "Elementos visuais chave no puzzle 2",
        "Compreendeu que tinha de pesar objetos",
        "Descobriu os controlos sem ajuda",
        "Compreendeu como iniciar o puzzle 3",
        "Compreendeu o objetivo do puzzle 3",
        "O inimigo deu um bom desafio",
    ]
    diverging_bar_chart(
        records, COMPREHENSION_ITEMS, COMPREHENSION_SCALE, COMPREHENSION_LABELS,
        "Compreensão de mecânicas e objetivos",
        out_dir / "bloco1_compreensao.png", short_comp,
    )

    print("\n" + "=" * 70)
    print("BLOCO 2a - Subescalas GUESS-18")
    print("=" * 70)
    subscale_summary = {}
    for name in GUESS18_SUBSCALES:
        items = GUESS18_SUBSCALES[name]
        res = subscale_scores(records, items, GUESS_SCALE)
        subscale_summary[name] = res
        print(f"- {name:<22} | scores={res['scores']} | "
              f"mediana={res['median']} moda={res['mode']} dp={res['stdev']}")

    fig, ax = plt.subplots(figsize=(7, 4))
    names = list(subscale_summary.keys())

    medians = []
    for name in names:
        medians.append(subscale_summary[name]["median"])

    # o desvio padrão dos scores por participante é mostrado como barra de
    # erro em torno da mediana de cada subescala (0 quando não definido)
    stdevs = []
    for name in names:
        value = subscale_summary[name]["stdev"]
        if value:
            stdevs.append(value)
        else:
            stdevs.append(0)

    ax.barh(names, medians, xerr=stdevs, color="#2166ac",
            error_kw=dict(ecolor="#555555", capsize=3, elinewidth=1))
    ax.set_xlim(1, 5)
    ax.set_xlabel("Mediana da subescala (1-5), barras de erro = desvio padrão")
    ax.set_title("Subescalas GUESS-18 (itens originais completos)",
                  fontsize=11, fontweight="bold")
    for i in range(len(names)):
        v = medians[i]
        sd = stdevs[i]
        ax.text(min(v + sd + 0.07, 4.75), i, f"{v} (dp={sd})",
                va="center", fontsize=8)
    fig.tight_layout()
    fig.savefig(out_dir / "bloco2a_subescalas_guess18.png", dpi=200,
                bbox_inches="tight")
    plt.close(fig)
    print(f"Gráfico guardado em: {out_dir / 'bloco2a_subescalas_guess18.png'}")

    all_guess_items = []
    for items in GUESS18_SUBSCALES.values():
        for item in items:
            all_guess_items.append(item)

    all_guess_short = [
        "Controlos diretos", "Interface fácil de navegar",
        "Desligado do mundo exterior", "Não quer verificar mundo real",
        "O jogo é divertido", "Sente-se aborrecido (inv.)",
        "Permite ser imaginativo", "Sente-se criativo",
        "Gosta dos efeitos sonoros", "Áudio melhora experiência",
        "Gosta dos gráficos", "Acha visualmente apelativo",
    ]
    diverging_bar_chart(
        records, all_guess_items, GUESS_SCALE, GUESS_LABELS,
        "Itens GUESS-18 (subescalas completas)",
        out_dir / "bloco2a_itens_guess18.png", all_guess_short,
    )

    print("\n" + "=" * 70)
    print("BLOCO 2b - Itens adicionais (fora do GUESS-18 original)")
    print("=" * 70)
    for theme in CUSTOM_ITEMS:
        items = CUSTOM_ITEMS[theme]
        print(f"\n[{theme}]")
        for item in items:
            s = item_stats(records, item, GUESS_SCALE)
            print(f"  - {item[:55]:<55} | mediana={s['median']} "
                  f"moda={s['mode']} dp={s['stdev']} n={s['n']}/{n}")

    all_custom_items = []
    for items in CUSTOM_ITEMS.values():
        for item in items:
            all_custom_items.append(item)

    all_custom_short = [
        "O jogo é atmosférico", "Sensação de regressar de uma viagem",
        "Enjoo durante o jogo", "Enjoo após jogar",
        "Narrativa fácil de entender",
        "Fácil navegar o mapa", "Fácil entender o que fazer/onde ir",
        "Resolver puzzles traz alegria", "Sentiu-se satisfeito",
        "Foi uma perda de tempo (inv.)", "Sentiu que completou um desafio",
    ]
    diverging_bar_chart(
        records, all_custom_items, GUESS_SCALE, GUESS_LABELS,
        "Itens adicionais específicos do projeto",
        out_dir / "bloco2b_itens_adicionais.png", all_custom_short,
    )

    print("\n" + "=" * 70)
    print("RESPOSTAS ABERTAS")
    print("=" * 70)
    open_fields = [
        "What did you like the most about Ego Fractum",
        "What did you least liked about Ego Fractum",
        "Aditional Sugestions:",
    ]
    for field in open_fields:
        print(f"\n[{field}]")
        for r in records:
            val = r.get(field)
            if val and val != "None":
                print(f"  - {val}")

    print("\nAnálise concluída. Gráficos guardados em:", out_dir.resolve())


if __name__ == "__main__":
    if len(sys.argv) > 1:
        xlsx = sys.argv[1]
    else:
        xlsx = "questionnaire.xlsx"
    main(xlsx)