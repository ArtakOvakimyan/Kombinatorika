from queue import Queue

moves = [(-2, 1), (-2, -1), (1, -2), (-1, -2), (2, -1), (2, 1), (1, 2), (-1, 2)]


def read(input_file):
    with open(input_file, 'r') as file:
        init_point = file.readline()
        init_point = (ord(init_point[0]) % 8, int(init_point[1]))
        finish_point = file.readline()
        finish_point = (ord(finish_point[0]) % 8, int(finish_point[1]))
    return init_point, finish_point


def make_steps(point):
    return list(map(lambda x: tuple(map(lambda i, j: i + j, point, x)), moves))


def point_is_valid(point):
    return 1 <= point[0] <= 8 and 1 <= point[1] <= 8


def point_is_dangerous(point, pawn):
    return point in [(pawn[0] - 1, pawn[1] - 1), (pawn[0] + 1, pawn[1] - 1)]


def bfs(start, finish):
    if start == finish:
        return start
    visited = set()
    visited.add(start)
    paths = dict()
    paths[start] = (start, )
    queue = Queue()
    queue.put(start)

    while not queue.empty():
        point = queue.get()
        if not point_is_valid(point):
            continue
        if point_is_dangerous(point, finish) and point != start:
            continue

        for step in make_steps(point):
            if step not in visited:
                queue.put(step)
                visited.add(step)
                paths[step] = (*paths[point], step)
                if step == finish:
                    return list(paths[step])


def write(output_file, path):
    with open(output_file, 'w') as file:
        for i, step in enumerate(path):
            if i == len(path) - 1:
                file.write(f"{chr(step[0] + 96)}{step[1]}")
            else:
                file.write(f"{chr(step[0] + 96)}{step[1]}\n")


if __name__ == '__main__':
    write("out.txt", bfs(*read("in.txt")))
