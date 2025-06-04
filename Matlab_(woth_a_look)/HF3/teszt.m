
clear; clc;
%HF1 HF3
%tridiagonal
%%  4x4-es ismert mátrix
disp('Teszt 1: 4x4-es  mátrix');

a = [2 2 2 2];             % főátló
b = [-1 -1 -1];            % alsó mellékátló
g = [-1 -1 -1];            % felső mellékátló
x = linspace(0, 4, 100)';  % kiértékelési pontok



% Karakterisztikus polinom kiértékelése
p = tridiagonalpoly(b, a, g, x);

% MATLAB karakterisztikus polinom 
n = length(a);
p_check = zeros(size(x));
for i = 1:length(x)
    T = diag(a - x(i)) + diag(b, -1) + diag(g, 1);
    p_check(i) = det(T);
end


figure;
plot(x, p, 'b-', x, p_check, 'r--');
legend('tridiagonalpoly', 'det(A - xI)');
title('Teszt 1: Összehasonlítás 4x4 mátrixon');
xlabel('x'); ylabel('Polinom érték');
grid on;

%% 6x6 véletlen tridiagonális mátrix
disp('Teszt 2: 6x6 véletlen tridiagonális mátrix');

rng(1); 
n = 6;
a = rand(1, n) * 10;
b = rand(1, n-1) * 5;
g = rand(1, n-1) * 5;
x = linspace(min(a)-5, max(a)+5, 200)';

% Értékelés
p = tridiagonalpoly(b, a, g, x);
p_check = zeros(size(x));
for i = 1:length(x)
    T = diag(a - x(i)) + diag(b, -1) + diag(g, 1);
    p_check(i) = det(T);
end

% Ábra
figure;
plot(x, p, 'b-', x, p_check, 'r--');
legend('tridiagonalpoly', 'det(A - xI)');
title('Teszt 2: 6x6 véletlen mátrix');
xlabel('x'); ylabel('Polinom érték');
grid on;

%%  tetszőleges méretű mátrix generálása és tesztelése

disp('Teszt 3: Generált példa');

m = 8; % tetszőleges méret
a = 5 + rand(1, m);             % diagonális
b = -1 + rand(1, m-1);          % alsó
g = -1 + rand(1, m-1);          % felső
x = linspace(0, 10, 300)';

p = tridiagonalpoly(b, a, g, x);
p_check = zeros(size(x));
for i = 1:length(x)
    T = diag(a - x(i)) + diag(b, -1) + diag(g, 1);
    p_check(i) = det(T);
end

% Ábra
figure;
plot(x, p, 'b-', x, p_check, 'r--');
legend('tridiagonalpoly', 'det(A - xI)');
title('Teszt 3: 8x8 generált mátrix');
xlabel('x'); ylabel('Polinom érték');
grid on;

disp(' Minden teszt lefutott.');

%gergorin körök





disp('--- Gersgorin-körök tesztelése ---');

%% péda mátrix gen
function A = general_valos_pelda(n, erosebb_diag)
   
    A = randi([-5, 5], n);  
    if erosebb_diag
        % Főátló elemeit megerősítjük, hogy Gersgorin-körök értelmesek legyenek
        A = A + diag(n * ones(1,n));
    end
end

%% --- 1. Teszt: 4x4-es kézzel megadott nem triviális mátrix ---
disp('Teszt 1: Kézzel megadott 4x4-es valós mátrix');
A1 = [10 -1 2 0; 1 8 0 1; 0 -1 5 2; 1 0 0 6];
[kp1, r_sor1, r_oszl1] = gersgorin_korok(A1);

% Ellenőrzés: sajátértékek a körökön belül?
eig1 = eig(A1);
ellenorzes1 = all(any(abs(eig1 - kp1.') <= r_sor1.', 2));
disp(['   Sajátértékek benne vannak legalább egy sor-körben? ', logical_to_str(ellenorzes1)]);



%% --- 2. Teszt: 5x5 véletlen mátrix erős főátlóval ---
disp('Teszt 2: 5x5 véletlen mátrix erős főátlóval');
A2 = general_valos_pelda(5, true);
[kp2, r_sor2, r_oszl2] = gersgorin_korok(A2);
eig2 = eig(A2);
ellenorzes2 = all(any(abs(eig2 - kp2.') <= r_sor2.', 2));
disp(['   Sajátértékek benne vannak legalább egy sor-körben? ', logical_to_str(ellenorzes2)]);




%% --- 3. Teszt: 7x7 teljesen véletlen mátrix ---
disp('Teszt 3: 7x7 teljesen véletlen valós mátrix');
A4 = general_valos_pelda(7, true);
[kp4, r_sor4, r_oszl4] = gersgorin_korok(A4);
eig4 = eig(A4);
ellenorzes4 = all(any(abs(eig4 - kp4.') <= r_sor4.', 2));
disp(['   Sajátértékek benne vannak legalább egy sor-körben? ', logical_to_str(ellenorzes4)]);

disp('--- Tesztek vége ---');

%% --- Lokális logikai sztring átalakító ---
function str = logical_to_str(logval)
    if logval
        str = 'IGEN';
    else
        str = 'NEM';
    end
end
% Random paritású függvény
n = floor(rand*7)+3 ;
p = [rand(1,1)*10+1,rand(1,n)*10];
x = linspace(-2,2,n+1);
y = polyval(p,x);
pp = spline(x,y);
xx = linspace(-2,2,1000);
yy = HFP8(x,y,xx);
figure
plot(xx, yy, 'b', 'LineWidth', 1.5);
hold on;
plot(xx, polyval(p, xx), 'r--', 'LineWidth', 1.5);
plot(xx, ppval(pp, xx), 'g-.', 'LineWidth', 1.5);
hold off;

% Címek és címkék hozzáadása
title('Random Parity Function Approximation', 'FontSize', 14);
xlabel('x', 'FontSize', 12);
ylabel('y', 'FontSize', 12);
legend('HFP8 Interpolation', 'Original', 'Spline Interpolation', 'Location', 'Best');
grid on;

% Ellentétes paritású függvény
n = n-1;
p = [rand(1,1)*10+1,rand(1,n)*10];
x = linspace(-2,2,n+1);
y = polyval(p,x);
pp = spline(x,y);

xx = linspace(-2,2,1000);
yy = HFP8(x,y,xx);
figure
plot(xx, yy, 'b', 'LineWidth', 1.5);
hold on;
plot(xx, polyval(p, xx), 'r--', 'LineWidth', 1.5);
plot(xx, ppval(pp, xx), 'g-.', 'LineWidth', 1.5);
hold off;

% Címek és címkék hozzáadása
title('Opposite Parity Function Approximation', 'FontSize', 14);
xlabel('x', 'FontSize', 12);
ylabel('y', 'FontSize', 12);
legend('HFP8 Interpolation', 'Original', 'Spline Interpolation', 'Location', 'Best');
grid on;

% Sin közelítés
xx = linspace(0, 2*pi, 100);
x = [0, pi/2, pi, 3*pi/2, 2*pi];
y = sin(x);
yy = HFP8(x, y, xx);
figure
plot(xx, yy, 'b', 'LineWidth', 1.5);
hold on;
plot(xx, sin(xx), 'r--', 'LineWidth', 1.5);
hold off;

% Címek és címkék hozzáadása
title('Sine Function Approximation', 'FontSize', 14);
xlabel('x', 'FontSize', 12);
ylabel('y', 'FontSize', 12);
legend('HFP8 Interpolation', 'Sine Function', 'Location', 'Best');
grid on;
