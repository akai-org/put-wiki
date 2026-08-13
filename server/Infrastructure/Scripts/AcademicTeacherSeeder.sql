-- Seed script for academic_teachers table (PostgreSQL dialect)
-- Idempotent script: Safe to run multiple times without duplicating records

INSERT INTO academic_teachers (
    id,
    usos_id,
    degrees,
    name,
    photo_url,
    email,
    phone_number,
    website_url,
    description
)
VALUES
    (
        '01914c2b-6f8e-7231-8000-0123456789ab'::uuid,
        'USOS-1001',
        ARRAY['dr hab.', 'inż.']::text[],
        'Anna Kowalska',
        'https://example.com/photos/akowalska.jpg',
        'a.kowalska@university.edu.pl',
        '+48 61 829 1001',
        'https://akowalska.university.edu.pl',
        'Associate Professor in the Department of Computer Science, specializing in Machine Learning and Data Science.'
    ),
    (
        '01914c2b-6f8e-7231-8000-0123456789ac'::uuid,
        'USOS-1002',
        ARRAY['prof. dr hab.']::text[],
        'Jan Nowak',
        NULL,
        'j.nowak@university.edu.pl',
        '+48 61 829 1002',
        NULL,
        'Head of Software Engineering Research Group. Conducting research on distributed systems and Clean Architecture.'
    ),
    (
        '01914c2b-6f8e-7231-8000-0123456789ad'::uuid,
        'USOS-1003',
        ARRAY['dr', 'mgr inż.']::text[],
        'Piotr Wiśniewski',
        'https://example.com/photos/pwisniewski.jpg',
        'p.wisniewski@university.edu.pl',
        NULL,
        'https://pwisniewski.dev',
        'Lecturer and researcher focused on Cloud Computing, DevOps, and modern C# / .NET development.'
    ),
    (
        '01914c2b-6f8e-7231-8000-0123456789ae'::uuid,
        'USOS-1004',
        ARRAY['mgr']::text[],
        'Maria Zielińska',
        NULL,
        'm.zielinska@university.edu.pl',
        '+48 61 829 1004',
        NULL,
        NULL
    )
ON CONFLICT (usos_id) DO NOTHING;