def read(input_file):
    initial_matrix = []
    with open(input_file, 'r') as file:
        dimension = int(file.readline())
        for _ in range(dimension):
            string = file.readline()
            initial_matrix.append([int(e) for e in string.split()])
    return initial_matrix, dimension


matrix, n = read("in.txt")
comps = [[] for _ in range(n)]


def dfs(v, visited, num):
    visited[v] = 1
    comps[num].append(v)
    for i in range(n):
        if matrix[v][i] == 1 and not visited[i]:
            dfs(i, visited, num)


def count():
    visited = [0] * n
    counter = 0
    for ind in range(n):
        if not visited[ind]:
            dfs(ind, visited, counter)
            counter += 1
    return counter


def write(output_file, amount, comps):
    with open(output_file, 'w') as file:
        file.write(f"{amount}\n")
        for i, comp in enumerate(comps):
            comp.sort()
            for v in comp:
                file.write(f"{v + 1} ",)
            file.write("0")
            if i != len(comps) - 1:
                file.write("\n")


if __name__ == '__main__':
    c = count()
    components = [k for k in comps if len(k) != 0]
    write("out.txt", c, components)
