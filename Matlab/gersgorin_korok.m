function [kp, r_sor, r_oszl] = gersgorin_korok(A)
    % Ellenőrzés
        
    [n,m] = size(A);
    if n~=m
        error('A mátrixnak négyzetesnek kell lennie.');
    end
    if ~ismatrix(A) 
        error('A mátrixnak Szimetrikusnak kell lennie.');
    end

    theta = linspace(0, 2*pi, 100);  % 0-tól 2pi-ig vektor
    
    kp = diag(A);      % Középpontok: a főátló elemei
    r_sor = zeros(n,1);
    r_oszl = zeros(n,1);
    
    figure;
    hold on;
    axis equal;
    title('Gersgorin-körök (sorok: piros, oszlopok: kék)');
    xlabel('Re');
    ylabel('Im');
    
    % Sor szerinti körök (piros)
    for i = 1:n
        r_sor(i) = sum(abs(A(i, [1:i-1, i+1:end]))); 
        x = kp(i) + r_sor(i) * cos(theta); %x-et el kell tolni a kör kp-val
        y =  r_sor(i) * sin(theta);
        fill(x, y, 'r', 'FaceAlpha', 0.3, 'EdgeColor', 'r');
    end
    
    % Oszlop szerinti körök (kék)
    for j = 1:n
        kp_oszlop = A(j,j);
        r_oszl(j) = sum(abs(A([1:j-1, j+1:end], j)));
        x = kp_oszlop + r_oszl(j) * cos(theta);
        y = r_oszl(j) * sin(theta);
        fill(x, y, 'b', 'FaceAlpha', 0.3, 'EdgeColor', 'b');
    end
    
    % Középpontok megjelenítése
    plot(kp, 0, 'ko', 'MarkerFaceColor', 'k', 'DisplayName', 'Középpontok');
    
    hold off;
end